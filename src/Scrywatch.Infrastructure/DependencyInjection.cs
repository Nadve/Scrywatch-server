using Dapper;
using Ardalis.SmartEnum.Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Scrywatch.Core.Auth;
using Scrywatch.Core.Cards;
using Scrywatch.Core.Interests;
using Scrywatch.Core.ValueObjects;
using Scrywatch.Core.Notifications;
using Scrywatch.Infrastructure.Auth;
using Scrywatch.Infrastructure.Cards;
using Scrywatch.Infrastructure.Notifications;
using Scrywatch.Persistence;
using Scrywatch.Infrastructure.Interests;

namespace Scrywatch.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        SqlMapper.AddTypeHandler(typeof(Finish), new SmartEnumByNameTypeHandler<Finish>());
        SqlMapper.AddTypeHandler(typeof(Currency), new SmartEnumByNameTypeHandler<Currency>());
        SqlMapper.AddTypeHandler(typeof(Intention), new SmartEnumByNameTypeHandler<Intention>());

        services.AddIdentity<User, UserRole>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddUserStore<UserStore>()
            .AddRoleStore<RoleStore>()
            .AddDefaultTokenProviders();

        services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        services.AddScoped(x => {
            var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext
                ?? throw new ApplicationException($"{nameof(ActionContextAccessor.ActionContext)} is null");
            var factory = x.GetRequiredService<IUrlHelperFactory>();
            return factory.GetUrlHelper(actionContext);
        });

        return services
            .AddScoped<IAuthService, AuthService>()
            .AddSingleton<IDbConnection, DbConnection>()
            .AddSingleton<ICardRepository, CardRepository>()
            .AddSingleton<IMailService, MailService>()
            .AddSingleton<IInterestRepository, InterestRepository>();
    }
}
