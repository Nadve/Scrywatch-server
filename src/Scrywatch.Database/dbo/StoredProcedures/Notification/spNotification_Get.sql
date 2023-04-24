CREATE PROCEDURE [dbo].[spNotification_Get]
AS
BEGIN
SELECT [Notification].InterestId,
	[User].Email,
	[Interest].CardId,
	[Finish].Value AS Finish,
	[Currency].Value AS Currency,
	[Intention].Value AS Intention,
	[Interest].Goal
FROM [Notification]
INNER JOIN [Interest] ON [Notification].InterestId = [Interest].Id
INNER JOIN [User] ON [Interest].UserId = [User].Id
INNER JOIN [Finish] ON [Interest].FinishId = [Finish].Id
INNER JOIN [Currency] ON [Interest].CurrencyId = [Currency].Id
INNER JOIN [Intention] ON [Interest].IntentionId = [Intention].Id
WHERE Created = CAST(GETDATE() AS DATE)
END
