using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scrywatch.Core.Configuration;
using Scrywatch.Infrastructure;
using Scrywatch.MergeService;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
{
    // mapping appsettings.json to Core.Configuration
    var mailSection = builder.Configuration.GetSection(MailConfiguration.SectionKey);
    var authTokenSection = builder.Configuration.GetSection(AuthTokenConfiguration.SectionKey);
    var clientSection = builder.Configuration.GetSection(ClientConfiguration.SectionKey);
    builder.Services
        .Configure<MailConfiguration>(mailSection)
        .Configure<ClientConfiguration>(clientSection)
        .Configure<AuthTokenConfiguration>(authTokenSection);

    builder.Services
        .AddInfrastructure()
        .AddMergeService()
        .AddAuthorization()
        .AddControllers();


    var authTokenConfig = authTokenSection.Get<AuthTokenConfiguration>()
        ?? throw new ApplicationException($"{nameof(AuthTokenConfiguration)} not found in appsettings.json");
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateActor = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = authTokenConfig.Issuer,
                ValidAudience = authTokenConfig.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authTokenConfig.Key))
            };
        });

}

var app = builder.Build();
{
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseCors(options =>
    {
        options.AllowAnyHeader();
        options.AllowAnyMethod();
        options.AllowAnyOrigin();
    });
    app.MapControllers();
    app.Run();
}
