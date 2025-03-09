using Microsoft.OpenApi.Models;

namespace Swagabond.Core.ObjectModel;

/// <summary>
/// A link to external documentation regarding the API
/// </summary>
public class ApiExternalLink
{
    /// <summary>
    /// A brief description of the link
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// The URL for the link 
    /// </summary>
    public string Url { get; set; } = string.Empty;

    public static ApiExternalLink FromOpenApi(OpenApiExternalDocs documentExternalDocs)
    {
        var link = new ApiExternalLink();
        
        if (documentExternalDocs is null)
            return link;
        
        link.Description = documentExternalDocs.Description ?? string.Empty;
        link.Url = documentExternalDocs.Url?.ToString() ?? string.Empty;
        
        return link;
    }

}