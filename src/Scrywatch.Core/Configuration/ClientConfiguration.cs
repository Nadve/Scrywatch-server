namespace Scrywatch.Core.Configuration;

public class ClientConfiguration
{
    public const string SectionKey = "Client";

    public string EmailConfirmedUrl { get; set; } = null!;
}
