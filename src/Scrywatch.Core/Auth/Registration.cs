namespace Scrywatch.Core.Auth;

public sealed class Registration
{
    public bool Success { get; set; } = true;

    public List<string> Errors { get; set; } = new();

    public Registration Failed(IEnumerable<string> errors)
    {
        Success = false;
        foreach (var error in errors)
        {
            Errors.Add(error);
        }

        return this;
    }

    public Registration Failed(string error)
    {
        Success = false;
        Errors.Add(error);

        return this;
    }
}
