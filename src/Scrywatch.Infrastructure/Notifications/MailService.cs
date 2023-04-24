using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Scrywatch.Core.Configuration;
using Scrywatch.Core.Notifications;

namespace Scrywatch.Infrastructure.Notifications;

public class MailService : IMailService
{
    private readonly MailConfiguration _mailConfig;

    public MailService(IOptions<MailConfiguration> mailConfig) =>
        _mailConfig = mailConfig.Value;

    public async Task SendEmailAsync(MailRequest mailRequest)
    {
        var email = new MimeMessage();
        email.To.Add(MailboxAddress.Parse(mailRequest.To));
        email.From.Add(MailboxAddress.Parse(_mailConfig.User));
        email.Subject = mailRequest.Subject;
        email.Body = new BodyBuilder
        {
            HtmlBody = mailRequest.Body
        }
        .ToMessageBody();

        using var smtp = new SmtpClient();
        smtp.Connect(_mailConfig.Host, _mailConfig.Port, SecureSocketOptions.StartTls);
        smtp.Authenticate(_mailConfig.User, _mailConfig.Password);
        await smtp.SendAsync(email);
        smtp.Disconnect(true);
    }
}
