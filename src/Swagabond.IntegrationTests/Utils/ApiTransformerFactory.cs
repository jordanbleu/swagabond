using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Swagabond.Core.Extensions;
using Swagabond.ObjectModelV1.Transformer;
using Swagabond.Templates.Extensions;

namespace Swagabond.IntegrationTests.Utils;




public static class ApiTransformerFactory
{
    /// <summary>
    /// Returns a service provider with all of Swagabond's dependencies 
    /// </summary>
    /// <returns></returns>
    public static ServiceProvider CreateV1Transformer()
    {
        // create a di container 
        var services = new ServiceCollection();

        // add logging
        services.AddLogging();

        // core logic 
        services.AddSwagabondObjectMapper();
        services.AddSwagabondTemplateEngineFactory();
        
        return services.BuildServiceProvider();
    }
}