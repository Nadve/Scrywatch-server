using Scrywatch.MergeService;
using Scrywatch.Infrastructure;
using Scrywatch.Core.Configuration;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration config = hostContext.Configuration;
        var mailSection = config.GetSection(MailConfiguration.SectionKey);
        services.Configure<MailConfiguration>(mailSection);

        services.AddInfrastructure()
                .AddMergeService();
    })
    .Build();

host.Run();
