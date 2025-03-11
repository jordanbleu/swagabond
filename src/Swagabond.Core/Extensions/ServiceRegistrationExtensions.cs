using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Readers;
using Swagabond.Core.Mappers;

namespace Swagabond.Core.Extensions;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddSwagabondObjectMapper(this IServiceCollection services)
    {
        // Use the entry application's logger if it exists, otherwise create a no-op one.
        if (services.All(s => s.ServiceType != typeof(ILoggerFactory)))
        {
            services.AddLogging();
        }

        services.AddTransient<OpenApiStreamReader>();
        services.AddTransient<OpenApiMapper>();
        
        return services;
    }
}