using Scrywatch.Core.Interests;
using Scrywatch.Core.ValueObjects;

namespace Scrywatch.Api.Interests;

public sealed record CreateInterest(
    int CardId,
    Finish Finish,
    Currency Currency,
    Intention Intention);
