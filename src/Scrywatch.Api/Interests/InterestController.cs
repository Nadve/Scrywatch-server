using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scrywatch.Core.Auth;
using Scrywatch.Core.Interests;

namespace Scrywatch.Api.Interests;

[ApiController]
[Route("[controller]")]
public sealed class InterestController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IInterestRepository _interestRepository;

    public InterestController(IAuthService authService, IInterestRepository interestRepository)
    {
        _authService = authService;
        _interestRepository = interestRepository;
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Get()
    {
        var userId = await _authService.GetUserId(HttpContext.User);
        if (userId is null)
            return Problem("Authentication failed.", statusCode: 401);

        return Ok(await _interestRepository.Get(userId));
    }

    [HttpPost("create")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Create([FromBody]CreateInterest request)
    {
        var userId = await _authService.GetUserId(HttpContext.User);
        if (userId is null)
            return Problem("Authentication failed.", statusCode: 401);

        var interest = new InterestDto
        {
            UserId = userId,
            CardId = request.CardId,
            Finish = request.Finish,
            Currency = request.Currency,
            Intention = request.Intention,
            Goal = request.Intention == Intention.Sell ? 100 : -100
        };

        if (await _interestRepository.Exists(interest))
            return Problem("You already have this interest.");

        return Ok(await _interestRepository.Create(interest));
    }

    [HttpPost("delete")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Delete([FromBody]int[] ids)
    {
        var userId = await _authService.GetUserId(HttpContext.User);
        if (userId is null)
            return Problem("Authentication failed.", statusCode: 401);

        foreach(var id in ids)
        {
            var interest = await _interestRepository.FindById(id);

            if (interest is null)
                return Problem("Interest doesn't exist.");

            if (interest.UserId != userId)
                return Problem("You are not authorized to delete this interest.");

            await _interestRepository.Delete(id);
        }

        return Ok();
    }

    [HttpPost("update")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Update([FromBody] UpdateInterest[] requests)
    {
        var userId = await _authService.GetUserId(HttpContext.User);
        if (userId is null)
            return Problem("Authentication failed.", statusCode: 401);

        foreach(var request in requests)
        {
            var interest = await _interestRepository.FindById(request.Id);

            if (interest is null)
                return Problem("Interest doesn't exist.");

            if (interest.UserId != userId)
                return Problem("You are not authorized to modify this interest.");

            await _interestRepository.Update(request.Id, request.Goal);
        }

        return Ok();
    }
}
