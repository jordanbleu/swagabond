using Microsoft.OpenApi.Models;

namespace Swagabond.Core.ObjectModel;

/// <summary>
/// Tags are used to group API operations together
/// </summary>
public class ApiTag
{
    /// <summary>
    /// Name of the tag / group
    /// </summary>
    public string Name { get; set; } = string.Empty;
   
    /// <summary>
    /// Description of the tag / group
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Link to documentation related to the tag
    /// </summary>
    public ApiExternalLink ExternalDocs { get; set; } = new();
    
    /// <summary>
    /// Operations under this tag
    /// </summary>
    public List<ApiOperation> Operations { get; set; } = new();
    
    public static ApiTag FromOpenApi(OpenApiTag openApiTag, OpenApiDocument openApiDocument, Api api)
    {
        var tag = new ApiTag();
        
        tag.Name = openApiTag.Name;
        tag.Description = openApiTag.Description ?? string.Empty;
        tag.ExternalDocs = ApiExternalLink.FromOpenApi(openApiTag.ExternalDocs);
        
        //
        // Crawl each operation in the Api and find places where this tag is used
        //
        // This is inefficient because it means we have to iterate through operations and paths twice and we are duplicating data.
        // Tools like SwaggerUI use tags to group operations, but that's not technically part of the OpenAPI spec (which i'm guessing 
        // is why it's not structured that way in the OpenAPI.NET library).
        // So this will create a fake hierarchy of Tags -> Paths -> Operations in case the user wants to generate clients that way.
        //
        foreach (var path in openApiDocument.Paths)
        {
            foreach (var operation in path.Value.Operations)
            {
                if (operation.Value.Tags.Any(t => t.Name == tag.Name))
                {
                    var rebuiltApiPath = ApiPath.FromOpenApi(path, openApiDocument, api);
                    tag.Operations.Add(ApiOperation.FromOpenApi(operation, openApiDocument, rebuiltApiPath));
                }
            }
        }
        
        return tag;
    }
    
    
}