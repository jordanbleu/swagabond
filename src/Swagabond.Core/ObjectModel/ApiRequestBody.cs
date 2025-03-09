using Microsoft.OpenApi.Models;

namespace Swagabond.Core.ObjectModel;

/// <summary>
/// The request body for the API.  Currently only one content type is supported and we assume that
/// the request body points to a schema definition.
/// </summary>
public class ApiRequestBody
{
    public string Description { get; set; } = string.Empty;
    public bool IsRequired { get; set; } = false;
    
    public ApiSchemaDefinition Schema { get; set; } = new();
    public ApiContentType ContentType { get; set; } = ApiContentType.PlainText;
    public string RawContentType { get; set; } = string.Empty;
    public static ApiRequestBody FromOpenApi(OpenApiRequestBody requestBody)
    {
        var apiRequestBody = new ApiRequestBody();
        
        apiRequestBody.Description = requestBody.Description ?? string.Empty;
        apiRequestBody.IsRequired = requestBody.Required;

        if (requestBody?.Content?.Any() == false)
            return apiRequestBody;
        
        // Currently we only support one content type per endpoint, so we just kinda grab the first one
        var content = requestBody.Content?.FirstOrDefault();

        if (content is null)
            return apiRequestBody;

        var rawContentType = content.Value.Key;
        apiRequestBody.RawContentType = rawContentType;
        apiRequestBody.ContentType = ApiContentTypeMapper.FromString(rawContentType);
        
        var contentBody = content.Value.Value;
        apiRequestBody.Schema = ApiSchemaDefinition.FromOpenApi(contentBody.Schema);
        
        return apiRequestBody;
    }
}