using Microsoft.Extensions.DependencyInjection;
using Swagabond.Templates.Engines;

namespace Swagabond.Templates.Extensions;

public static class ServiceRegistrationExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddSwagabondTemplateEngineFactory()
        {
            // Template Engines
            services.AddTransient<ITemplateEngine, ScribanTemplateEngine>();

            // Factory
            services.AddTransient<TemplateEngineFactory>();

            return services;
        }
    }
}