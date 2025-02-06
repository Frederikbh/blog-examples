using Azure.Identity;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DynamicSessions.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddDynamicSessions(this IServiceCollection services) =>
        services.AddScoped(
            sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var logger = sp.GetRequiredService<ILogger<DynamicSessionsClient>>();

                var endpoint = configuration["SessionPool:Endpoint"];

                if (string.IsNullOrWhiteSpace(endpoint))
                {
                    throw new InvalidOperationException("SessionPool:Endpoint is required.");
                }

                var options = new DynamicSessionsServiceOptions { Endpoint = endpoint };

                return new DynamicSessionsClient(options, httpClientFactory, new DefaultAzureCredential(), logger);
            });
}
