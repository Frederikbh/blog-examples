using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace BookStore.Api.Common.Extensions;

public static class WebApplicationExtensions
{
    public static void RegisterEndpoints(this WebApplication app, bool skipAuthOnLocal = false)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        var endpoints = assemblies
            .Where(e => e.FullName?.Contains("BookStore") ?? false)
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(IEndpoint).IsAssignableFrom(type))
            .Where(type => !type.IsAbstract)
            .Select(Activator.CreateInstance)
            .Cast<IEndpoint>()
            .ToList();

        foreach (var endpoint in endpoints)
        {
            var builtEndpoint = endpoint.Map(app);

            if (app.Environment.IsDevelopment() && skipAuthOnLocal)
            {
                builtEndpoint.AllowAnonymous();
            }
        }
    }
}
