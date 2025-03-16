using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;

namespace Swagabond.Core.ObjectModel;

/// <summary>
/// The parent/root object of an API spec.
/// </summary>
public class Api
{
    public string SpecVersion { get; set; } = string.Empty;

    public ApiSpecType Type { get; set; } = ApiSpecType.OpenApi;
    
    public ApiInfo Info { get; set; } = ApiInfo.Empty;
    
    public ApiExternalLink ExternalDocs { get; set; } = new();

    public List<ApiPath> Paths { get; set; } = new();

    public List<ApiSchema> Schemas { get; set; } = new();

    public Dictionary<string, string> Metadata { get; set; }  = new();
    
    /// <summary>
    /// Selects all operations from all paths on the api
    /// </summary>
    public IEnumerable<ApiOperation> Operations => Paths.SelectMany(p => p.Operations);

    public static Api Empty = new();
    
    public static Api FromOpenApi(OpenApiDocument document, OpenApiDiagnostic diag, MapperRequest mapperRequest)
    {
        var api = new Api
        {
            // Map the basics 
            SpecVersion = diag.SpecificationVersion.ToString(),
            Info = ApiInfo.FromOpenApi(document.Info),
            ExternalDocs = ApiExternalLink.FromOpenApi(document.ExternalDocs),
        };
        
        // Map paths 
        if (document.Paths?.Any() == true)
        {
            foreach (var openApiPath in document.Paths)
            {
                api.Paths.Add(ApiPath.FromOpenApi(openApiPath, document, api));
            }
        }
        
        // Map schemas
        if (document.Components?.Schemas?.Any() == true)
        {
            foreach (var schema in document.Components.Schemas)
            {
                api.Schemas.Add(ApiSchema.FromOpenApi(schema, api));
            }
        }

        api.Metadata = mapperRequest.Metadata;

        return api;
    }
}