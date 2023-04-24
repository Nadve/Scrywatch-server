using Microsoft.AspNetCore.Mvc;
using Scrywatch.Core.Cards;

namespace Scrywatch.Api.Cards;

[ApiController]
[Route("[controller]")]
public sealed class CardController : ControllerBase
{
    private readonly ICardRepository _cardRepository;

    public CardController(ICardRepository cardRepository) =>
        _cardRepository = cardRepository;

    [HttpGet("names")]
    public async Task<IActionResult> GetCardNames() =>
        Ok(await _cardRepository.GetCardNames());

    [HttpGet]
    public async Task<IActionResult> GetCards(string name) =>
        Ok(await _cardRepository.GetCardsByName(name));
}
