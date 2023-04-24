using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Scrywatch.Core.Auth;
using Scrywatch.Core.Configuration;
using Scrywatch.Core.Notifications;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace Scrywatch.Infrastructure.Auth;

public sealed class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IMailService _mailService;
    private readonly AuthTokenConfiguration _config;
    private readonly IUrlHelper _urlHelper;
    private readonly IHttpContextAccessor _http;

    public AuthService(UserManager<User> userManager, SignInManager<User> signInManager,
        IMailService mailService, IOptions<AuthTokenConfiguration> config, IUrlHelper url, IHttpContextAccessor http)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mailService = mailService;
        _config = config.Value;
        _urlHelper = url;
        _http = http;
    }

    public async Task<Confirmation> ConfirmEmail(string email, string token)
    {
        Confirmation confirmation = new();

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return confirmation.Failed("User does not exist.");

        var emailConfirmation = await _userManager.ConfirmEmailAsync(user, token);
        if (emailConfirmation.Succeeded) return confirmation;

        return confirmation.Failed(emailConfirmation
            .Errors
            .Select(e => e.Description)
            .Aggregate((string p, string n) =>
            $"{p}{Environment.NewLine}{n}"));
    }

    public async Task<string?> GetUserId(ClaimsPrincipal principal)
    {
        var email = principal.FindFirstValue(ClaimTypes.Email);
        if (email is null) return null;

        var user = await _userManager.FindByEmailAsync(email);
        if (user is null) return null;

        return user.Id;
    }

    public async Task<Registration> Register(string email, string password)
    {
        Registration registration = new();

        if (EmailIsNotValid(email))
        {
            return registration.Failed("Email isn't valid.");
        }

        var user = new User { UserName = email, Email = email };
        var createUser = await _userManager.CreateAsync(user, password);

        if (createUser.Succeeded)
        {
            User createdUser = await _userManager.FindByEmailAsync(email)
                ?? throw new ApplicationException("Failed to find created user");
            await SendEmailConfirmation(createdUser);
            return registration;
        }

        return registration.Failed(createUser.Errors.Select(e => e.Description));
    }

    public async Task<Signing> SignIn(string email, string password)
    {
        Signing signing = new();

        var signInUser = await _signInManager.PasswordSignInAsync(email, password, true, false);
        if (signInUser.Succeeded)
        {
            signing.Token = GenerateToken(email);
            return signing;
        }

        if (signInUser.IsNotAllowed)
        {
            User user = await _userManager.FindByEmailAsync(email)
                ?? throw new ApplicationException($"{nameof(UserManager<User>)} and {nameof(SignInManager<User>)} don't agree whether user exists or not.");

            await SendEmailConfirmation(user);

            return signing.Failed("You need to confirm your email address, before you'll be able to log in.");
        }

        return signing.Failed("User not found.");
    }

    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
    }

    private async Task SendEmailConfirmation(User user)
    {
        var protocol = _http.HttpContext?.Request.Scheme ?? "https";
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var confirmationLink = _urlHelper.Action("ConfirmEmail", "Auth", new { token, email =  user.Email }, protocol);
        var mail = new MailRequest
        {
            To = user.Email,
            Subject = "Scrywatch - email confirmation",
            Body = $"<a href=\"{confirmationLink}\">{confirmationLink}</a>"
        };
        await _mailService.SendEmailAsync(mail);
    }

    private string GenerateToken(string email)
    {
        var token = new JwtSecurityToken
        (
            issuer: _config.Issuer,
            audience: _config.Audience,
            expires: DateTime.Today.AddDays(30),
            claims: new[] { new Claim(JwtRegisteredClaimNames.Email, email) },
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Key)),
                SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static bool EmailIsNotValid(string email)
    {
        var trimmedEmail = email.Trim();
        if (trimmedEmail.EndsWith(".")) return true;

        try
        {
            var mail = new MailAddress(email);
            return !mail.Address.Equals(trimmedEmail);
        }
        catch
        {
            return true;
        }
    }
}
