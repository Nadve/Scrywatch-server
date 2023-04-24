namespace Scrywatch.Core.Notifications;

public interface IMailService
{
    Task SendEmailAsync(MailRequest mailRequest);
}
