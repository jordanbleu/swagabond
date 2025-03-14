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

    /// <summary>
    /// Global list of all tags used on the API
    /// </summary>
    public List<ApiTag> Tags { get; set; } = new();

    public List<ApiPath> Paths { get; set; } = new();

    public Dictionary<string, string> Metadata { get; set; }  = new();

    public static Api FromOpenApi(OpenApiDocument document, OpenApiDiagnostic diag, MapperRequest mapperRequest)
    {
        var api = new Api
        {
            // Map the basics 
            SpecVersion = diag.SpecificationVersion.ToString(),
            Info = ApiInfo.FromOpenApi(document.Info),
            ExternalDocs = ApiExternalLink.FromOpenApi(document.ExternalDocs),
        };

        // Map tags, and create a fake hierarchy of tags -> paths -> operations
        api.Tags = document.Tags.Select(t => ApiTag.FromOpenApi(t, document, api)).ToList();

        // Map paths 
        if (document.Paths?.Any() == true)
        {
            foreach (var openApiPath in document.Paths)
            {
                api.Paths.Add(ApiPath.FromOpenApi(openApiPath, document, api));
            }
        }


        api.Metadata = mapperRequest.Metadata;

        return api;
    }
}