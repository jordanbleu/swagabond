using Microsoft.OpenApi.Models;
using Swagabond.ObjectModelV1.Extensions;

namespace Swagabond.ObjectModelV1.Transformer;

public interface IApiV1Transformer
{
    ApiV1 FromOpenApi(TransformerV1Request v1Request, OpenApiDocument document, string apiSpecVersionString);
}

public class TransformerV1Request
{
    public Dictionary<string, string> Metadata { get; init; } = new();
}


public class ApiV1Transformer : IApiV1Transformer
{
    private readonly IPathV1Transformer _pathV1Transformer;
    private readonly IInfoV1Transformer _infoV1Transformer;
    private readonly IExternalDocsV1Transformer _externalDocsV1Transformer;
    private readonly ISchemaDefinitionV1Transformer _schemaDefinitionV1Transformer;

    public ApiV1Transformer(IPathV1Transformer pathV1Transformer, IInfoV1Transformer infoV1Transformer, IExternalDocsV1Transformer externalDocsV1Transformer, ISchemaDefinitionV1Transformer schemaDefinitionV1Transformer)
    {
        _pathV1Transformer = pathV1Transformer;
        _infoV1Transformer = infoV1Transformer;
        _externalDocsV1Transformer = externalDocsV1Transformer;
        _schemaDefinitionV1Transformer = schemaDefinitionV1Transformer;
    }

    public ApiV1 FromOpenApi(TransformerV1Request v1Request, OpenApiDocument document, string apiSpecVersionString)
    {
        var api = new ApiV1();

        api.IsEmpty = false;
        api.Metadata = v1Request.Metadata;
        
        var info = document.Info;
        api.Info = _infoV1Transformer.FromOpenApi(info);

        api.Name = info.Title.ToClassName();
        api.Title = info.Title;
        api.Description = info.Description;
        api.Version = info.Version;
        api.SpecVersion = apiSpecVersionString;
        api.ExternalDocumentationLink = _externalDocsV1Transformer.FromOpenApi(document.ExternalDocs);
        
        // paths
        //
        foreach (var kvp in document.Paths)
        {
            api.Paths.Add(_pathV1Transformer.FromOpenApi(kvp, api));
        }

        // schemas
        //
        foreach (var schema in document.Components.Schemas)
        {
            api.Schemas.Add(_schemaDefinitionV1Transformer.FromOpenApi(schema.Value, api));
        }

        api.Metadata = v1Request.Metadata;
        
        return api;
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}