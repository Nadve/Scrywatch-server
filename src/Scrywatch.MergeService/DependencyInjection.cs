using Polly;
using System.Net;
using Scrywatch.MergeService.Scryfall;

namespace Scrywatch.MergeService;

public static class DependencyInjection
{
    public static IServiceCollection AddMergeService(this IServiceCollection services)
    {
        services.AddHttpClient<IScryfallClient, ScryfallClient>()
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            })
            .AddPolicyHandler(request => Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(60)));

        services.AddHostedService<Worker>();
        return services;
    }
}
