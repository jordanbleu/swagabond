using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Readers;
using Swagabond.Core.Mappers;
using Swagabond.Core.Parsers;
using Swagabond.ObjectModelV1.Transformer;

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
        services.AddTransient<IMicrosoftSwaggerParser, MicrosoftSwaggerParser>();
        
        // transformers
        services.AddTransient<IDataTypeV1Transformer, DataTypeV1Transformer>();
        services.AddTransient<IEnumOptionV1Transformer, EnumOptionV1Transformer>();
        services.AddTransient<IExtensionV1Transformer, ExtensionV1Transformer>();
        services.AddTransient<IExternalDocsV1Transformer, ExternalDocsV1Transformer>();
        services.AddTransient<IInfoV1Transformer, InfoV1Transformer>();
        services.AddTransient<IOperationV1Transformer, OperationV1Transformer>();
        services.AddTransient<IPathV1Transformer, PathV1Transformer>();
        services.AddTransient<IRequestBodyV1Transformer, RequestBodyV1Transformer>();
        services.AddTransient<IResponseBodyV1Transformer, ResponseBodyV1Transformer>();
        services.AddTransient<ISchemaDefinitionV1Transformer, SchemaDefinitionV1Transformer>();
        services.AddTransient<ISchemaReferenceV1Transformer, SchemaReferenceV1Transformer>();
        services.AddTransient<IApiV1Transformer, ApiV1Transformer>();
        services.AddTransient<IServerV1Transformer, ServerV1Transformer>();
        
        // mapping
        services.AddTransient<OpenApiMapper>();
        
        
        return services;
    }
}