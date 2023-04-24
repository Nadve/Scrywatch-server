namespace Scrywatch.Core.Auth;

public sealed class Signing
{
    public string Token { get; set; } = string.Empty;

    public bool Success { get; set; } = true;

    public string Error { get; set; } = string.Empty;  

    public Signing Failed(string error)
    {
        Success = false;
        Error = error;

        return this;
    }
}
