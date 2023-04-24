namespace Scrywatch.Core.Notifications;

public sealed class MailRequest
{
    public string To { get; init; } = null!;

    public string Subject { get; init; } = null!;

    public string Body { get; init; } = null!;
}
