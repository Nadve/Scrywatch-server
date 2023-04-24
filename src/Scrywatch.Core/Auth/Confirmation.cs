namespace Scrywatch.Core.Auth;

public sealed class Confirmation
{
    public bool Success { get; set; } = true;

    public string Error { get; set; } = string.Empty;  

    public Confirmation Failed(string error)
    {
        Success = false;
        Error = error;

        return this;
    }
}
