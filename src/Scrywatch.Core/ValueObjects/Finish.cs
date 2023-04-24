using Ardalis.SmartEnum;
using Ardalis.SmartEnum.SystemTextJson;
using System.Text.Json.Serialization;

namespace Scrywatch.Core.ValueObjects;

[JsonConverter(typeof(SmartEnumNameConverter<Finish, int>))]
public sealed class Finish : SmartEnum<Finish>
{
    public static readonly Finish NonFoil = new(nameof(NonFoil), 1);
    public static readonly Finish Foil = new(nameof(Foil), 2);
    public static readonly Finish Etched = new(nameof(Etched), 3);
    public static readonly Finish Glossy = new(nameof(Glossy), 4);

    private Finish(string name, int value) : base(name.ToLower(), value) { }
}
