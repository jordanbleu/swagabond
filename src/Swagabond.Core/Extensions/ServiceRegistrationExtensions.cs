using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Readers;
using Swagabond.Core.Mappers;
using Swagabond.Core.Parsers;

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

        // stream reading / parsing
        services.AddTransient<OpenApiStreamReader>();
        services.AddTransient<IMicrosoftSwaggerParser>();
        
        // mapping
        services.AddTransient<OpenApiMapper>();
        
        return services;
    }
}