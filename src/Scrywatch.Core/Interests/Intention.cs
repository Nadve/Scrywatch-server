using Ardalis.SmartEnum;
using Ardalis.SmartEnum.SystemTextJson;
using System.Text.Json.Serialization;

namespace Scrywatch.Core.Interests;

[JsonConverter(typeof(SmartEnumNameConverter<Intention, int>))]
public sealed class Intention : SmartEnum<Intention>
{
    public static readonly Intention Buy = new(nameof(Buy), 1);
    public static readonly Intention Sell = new(nameof(Sell), 2);

    private Intention(string name, int value) : base(name.ToLower(), value) { }
}
