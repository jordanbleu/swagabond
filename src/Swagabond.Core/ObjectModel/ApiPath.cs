using Microsoft.OpenApi.Models;

namespace Swagabond.Core.ObjectModel;

public class ApiPath
{
    /// <summary>
    /// The full route but with slashes and curly braces removed.  Suitable for filenames, etc
    /// </summary>
    public string Name => Route.Replace("/", "_")
        .Replace("{", "")
        .Replace("}", "");
    
    /// <summary>
    /// The route of the api endpoints
    /// </summary>
    public string Route { get; set; } = string.Empty;
    
    /// <summary>
    /// A description of the api route
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// A summary of the endpoints under this route
    /// </summary>
    public string Summary { get; set; } = string.Empty;

    /// <summary>
    /// List of operations under this path.
    /// </summary>
    public List<ApiOperation> Operations { get; set; } = new();

    /// <summary>
    /// The API this path belongs to
    /// </summary>
    public Api Api { get; set; } = Api.Empty;

    public bool IsEmpty { get; set; } = true;

    public static ApiPath Empty = new();

    public static ApiPath FromOpenApi(KeyValuePair<string, OpenApiPathItem> path, OpenApiDocument document, Api api)
    {
        var apiPath = new ApiPath();
        var p = path.Value;
        
        apiPath.Route = path.Key;
        apiPath.Description = p.Description ?? string.Empty;
        apiPath.Summary = p.Summary ?? string.Empty;
        apiPath.Api = api;
        apiPath.IsEmpty = false;
        
        foreach (var operation in p.Operations)
        {
            apiPath.Operations.Add(ApiOperation.FromOpenApi(operation, document, api, apiPath));
        }
        
        return apiPath;
    }
}