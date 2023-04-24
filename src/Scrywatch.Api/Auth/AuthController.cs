using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Scrywatch.Api.Auth;
using Scrywatch.Core.Auth;
using Scrywatch.Core.Configuration;

namespace Scrywatch.Api.Users;

[ApiController]
[Route("[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ClientConfiguration _config;

    public AuthController(IAuthService authService, IOptions<ClientConfiguration> config)
    {
        _authService = authService;
        _config = config.Value;
    }

    [HttpGet("confirmEmail")]
    public async Task<IActionResult> ConfirmEmail(string email, string token)
    {
        var verification = await _authService.ConfirmEmail(email, token);
        return verification.Success
            ? Redirect(_config.EmailConfirmedUrl)
            : Problem(verification.Error);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody]Authentication request)
    {
        var registration = await _authService.Register(request.Email, request.Password);
        return registration.Success
            ? Ok(registration)
            : Problem(registration.Errors.Aggregate((p, n) => $"{p}{Environment.NewLine}{n}"));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]Authentication request)
    {
        var signing = await _authService.SignIn(request.Email, request.Password);
        return signing.Success
            ? Ok(new { token = signing.Token })
            : Problem(signing.Error);
    }

    [HttpPost("logout")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Logout()
    {
        await _authService.Logout();
        return Ok();
    }
}
