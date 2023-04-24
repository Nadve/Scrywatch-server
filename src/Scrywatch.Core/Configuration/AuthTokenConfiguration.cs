namespace Scrywatch.Core.Configuration;

public class AuthTokenConfiguration
{
    public const string SectionKey = "AuthToken";
    public string Key { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
}
