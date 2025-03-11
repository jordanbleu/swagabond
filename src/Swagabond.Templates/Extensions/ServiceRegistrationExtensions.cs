using Microsoft.Extensions.DependencyInjection;
using Swagabond.Templates.Engines;

namespace Swagabond.Templates.Extensions;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddSwagabondTemplateEngineFactory(this IServiceCollection services)
    {
        // Template Engines
        services.AddTransient<ITemplateEngine, ScribanTemplateEngine>();
        
        // Factory
        services.AddTransient<TemplateEngineFactory>();

        return services;
    }
    
}