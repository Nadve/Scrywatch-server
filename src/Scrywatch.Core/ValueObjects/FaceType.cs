using Ardalis.SmartEnum;

namespace Scrywatch.Core.ValueObjects;

public sealed class FaceType : SmartEnum<FaceType>
{
    public static readonly FaceType Front = new(nameof(Front), 1);
    public static readonly FaceType Back = new(nameof(Back), 2);

    private FaceType(string name, int value) : base(name.ToLower(), value) { }
}
