using Dapper;
using System.Data;
using System.Data.SqlClient;
using Scrywatch.MergeService.IO;
using Scrywatch.MergeService.Scryfall;
using Scrywatch.Core.ValueObjects;
using Scrywatch.Core.Cards;
using Scrywatch.Core.Interests;
using Scrywatch.Persistence;
using Scrywatch.MergeService.Database.Tables;
using Scrywatch.MergeService.Translation;
using Scrywatch.Core.Notifications;
using Scrywatch.Infrastructure.Notifications;
using Scrywatch.Persistence.Data;

namespace Scrywatch.MergeService;

public class Worker : BackgroundService
{
    private readonly IConfiguration _config;
    private readonly ILogger<Worker> _logger;
    private readonly IScryfallClient _scryfall;
    private readonly ICardRepository _cardRepository;
    private readonly IMailService _mailService;

    public Worker(IConfiguration config, ILogger<Worker> logger,
        IScryfallClient scryfallClient, ICardRepository cardRepository, IMailService mail)
    {
        _config = config;
        _logger = logger;
        _scryfall = scryfallClient;
        _cardRepository = cardRepository;
        _mailService = mail;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        SqlConnection sqlConnection;
        while (!stoppingToken.IsCancellationRequested)
        { 
            Bulk bulk = await _scryfall.GetBulkData(stoppingToken);
            DateTime bulkDate = bulk.AllCardsUpdated;
            _logger.LogInformation($"Scryfall data last updated: {bulkDate}");

            DateTime lastMerge = DateTime.Now;
            try
            {
                sqlConnection = new SqlConnection(_config.GetConnectionString("Default"));
                sqlConnection.Open();
                lastMerge = (await sqlConnection.QueryAsync<DateTime>(
                    StoredProcedure.GetLastMerge,
                    commandType: CommandType.StoredProcedure)).First();
                sqlConnection.Close();
                _logger.LogInformation($"Database last updated: {lastMerge}");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            _logger.LogInformation($"Scryfall updated: {bulkDate}");
            if(DateOnly.FromDateTime(bulkDate).Equals(DateOnly.FromDateTime(lastMerge)))
            {
                Thread.Sleep(TimeSpan.FromHours(4));
                continue;
            }

            var time = DateTime.Now;
            _logger.LogInformation("Fetching data from Scryfall...");
            var sets = await _scryfall.GetSetsAsync(stoppingToken);
            IEnumerable<Scryfall.Card> cards = await _scryfall.GetCardsAsync(stoppingToken);
            var dtos = cards
                .Where(c => c.Language.Equals("en"))
                .Distinct()
                .Translate(bulkDate);
            _logger.LogInformation($"Fetching took {DateTime.Now - time} time");

            _logger.LogInformation("Merging with database");
            /*
             * To pass a collection as a paramaterer to a stored procedure
             * we first need to transform the collection into a table(TVP)
             */
            var setTable = ScrywatchTable.Set.Create();
            foreach (var set in sets)
            {
                setTable.Rows.Add(set.Name, set.Code, set.Svg);
            }

            var cardNameTable = ScrywatchTable.CardName.Create();
            foreach (var name in dtos.Select(c => c.Name).Distinct())
            {
                cardNameTable.Rows.Add(name);
            }

            var cardTable = ScrywatchTable.Card.Create();
            var cardPriceTable = ScrywatchTable.CardPrice.Create();
            var cardFaceTable = ScrywatchTable.CardFace.Create();
            var cardFinishTable = ScrywatchTable.CardFinish.Create();
            foreach (var card in dtos)
            {
                cardTable.Rows.Add(
                    card.Id,
                    card.Name,
                    card.SetCode,
                    card.Rarity,
                    card.CollectorNumber);

                foreach (var price in card.Prices)
                {
                    cardPriceTable.Rows.Add(
                        card.Id,
                        price.Finish.Name,
                        price.Currency.Name,
                        DateTime.Parse(price.Date.ToString()),
                        price.Value);
                }

                foreach (var finish in card.Finishes)
                {
                    cardFinishTable.Rows.Add(
                        card.Id,
                        finish.Name);
                }

                if (card.FrontFaceUrl != null)
                {
                    cardFaceTable.Rows.Add(
                        card.Id,
                        FaceType.Front.Name,
                        card.FrontFaceUrl);
                }

                if (card.BackFaceUrl != null)
                {
                    cardFaceTable.Rows.Add(
                        card.Id,
                        FaceType.Back.Name,
                        card.BackFaceUrl);
                }
            }

            sqlConnection = new SqlConnection(_config.GetConnectionString("Default"));
            sqlConnection.Open();
            using var transaction = sqlConnection.BeginTransaction();
            try
            {
                IEnumerable<MergeOutput> mergeOutput = await sqlConnection.QueryAsync<MergeOutput>(
                    StoredProcedure.MergeSets,
                    new { sets = setTable.AsTableValuedParameter(setTable.TableName) },
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure);
                _logger.LogInformation($"{setTable.TableName}: {new MergeResult(mergeOutput)}");

                mergeOutput = await sqlConnection.QueryAsync<MergeOutput>(
                    StoredProcedure.MergeCardNames,
                    new { names = cardNameTable.AsTableValuedParameter(cardNameTable.TableName) },
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure);
                _logger.LogInformation($"{cardNameTable.TableName}: {new MergeResult(mergeOutput)}");

                mergeOutput = await sqlConnection.QueryAsync<MergeOutput>(
                    StoredProcedure.MergeCards,
                    new { cards = cardTable.AsTableValuedParameter(cardTable.TableName) },
                    commandTimeout: 120,
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure);
                _logger.LogInformation($"{cardTable.TableName}: {new MergeResult(mergeOutput)}");

                mergeOutput = await sqlConnection.QueryAsync<MergeOutput>(
                    StoredProcedure.MergeCardFinishes,
                    new { finishes = cardFinishTable.AsTableValuedParameter(cardFinishTable.TableName) },
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure);
                _logger.LogInformation($"{cardFinishTable.TableName}: {new MergeResult(mergeOutput)}");

                mergeOutput = await sqlConnection.QueryAsync<MergeOutput>(
                    StoredProcedure.MergeCardFaces,
                    new { faces = cardFaceTable.AsTableValuedParameter(cardFaceTable.TableName) },
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure);
                _logger.LogInformation($"{cardFaceTable.TableName}: {new MergeResult(mergeOutput)}");

                int changes = await sqlConnection.ExecuteAsync(
                    StoredProcedure.InsertCardPrices,
                    new { prices = cardPriceTable.AsTableValuedParameter(cardPriceTable.TableName) },
                    commandTimeout: 120,
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure);
                _logger.LogInformation($"{cardPriceTable.TableName}: rows {changes} inserted.");

                _ = await sqlConnection.ExecuteAsync(
                    StoredProcedure.UpdateMerged,
                    new { date = bulkDate },
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure);

                transaction.Commit();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            finally
            {
                sqlConnection.Close();
            }
            _logger.LogInformation("Merging finished.");

            /*
             * After the merge, we have to refresh(merge) the notifications
             * and notify users of interests that have met the set goal
             */
            sqlConnection = new SqlConnection(_config.GetConnectionString("Default"));
            sqlConnection.Open();
            IEnumerable<MergeOutput> notificationMerge = await sqlConnection.QueryAsync<MergeOutput>(
                StoredProcedure.MergeNotifications,
                commandTimeout: 120,
                commandType: CommandType.StoredProcedure);
            sqlConnection.Close();
            _logger.LogInformation($"Notification: {new MergeResult(notificationMerge)}");

            sqlConnection = new SqlConnection(_config.GetConnectionString("Default"));
            sqlConnection.Open();
            IEnumerable<NotificationData> notificationData = await sqlConnection.QueryAsync<NotificationData>(
                StoredProcedure.GetNotifications,
                commandType: CommandType.StoredProcedure);
            sqlConnection.Close();

            var users = notificationData.Select(n => n.Email).Distinct();
            List<dynamic> interests = new();
            foreach(var data in notificationData)
            {
                interests.Add(new
                {
                    User = data.Email,
                    Id = data.InterestId,
                    data.Intention,
                    Card = await _cardRepository.GetCard(data.CardId, data.Finish, data.Currency),
                    data.Goal
                });
            }
            List<Notification> notifications = new();
            foreach(var user in users)
            {
                notifications.Add(new Notification
                {
                    Email = user,
                    Interests = interests
                    .Where(i => i.User.Equals(user))
                    .Select(i => new Interest
                    {
                        Id = i.Id,
                        Intention = i.Intention,
                        Card = i.Card,
                        Goal = i.Goal
                    })
                });
            }

            foreach(var notification in notifications)
            {
                var mail = new MailRequest
                {
                    To = notification.Email,
                    Subject = "Scrywatch notification",
                    Body = new HtmlBuilder(notification.Interests).ToString()
                };
                await _mailService.SendEmailAsync(mail);
            }
        }
    }
}
