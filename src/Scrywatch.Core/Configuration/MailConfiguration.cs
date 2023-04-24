namespace Scrywatch.Core.Configuration;

public class MailConfiguration
{
    public const string SectionKey = "Mail";

    public string Name { get; set; } = null!;

    public string User { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Host { get; set; } = null!;

    public int Port { get; set; }
}
