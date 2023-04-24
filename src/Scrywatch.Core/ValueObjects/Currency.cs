using Ardalis.SmartEnum;
using Ardalis.SmartEnum.SystemTextJson;
using System.Text.Json.Serialization;

namespace Scrywatch.Core.ValueObjects;

[JsonConverter(typeof(SmartEnumNameConverter<Currency, int>))]
public abstract class Currency : SmartEnum<Currency>
{
    public static readonly Currency Eur = new EurCurrency();
    public static readonly Currency Usd = new UsdCurrency();
    public static readonly Currency Tix = new TixCurrency();

    private Currency(string name, int value) : base(name.ToLower(), value) { }

    public abstract string Symbol { get; }

    public sealed class EurCurrency : Currency
    {
        public EurCurrency() : base(nameof(Eur), 1) { }

        public override string Symbol => "€";
    }

    public sealed class UsdCurrency : Currency
    {
        public UsdCurrency() : base(nameof(Usd), 2) { }

        public override string Symbol => "$";
    }

    public sealed class TixCurrency : Currency
    {
        public TixCurrency() : base(nameof(Tix), 3) { }

        public override string Symbol => string.Empty;
    }
}
