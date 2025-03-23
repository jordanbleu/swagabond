using Microsoft.OpenApi.Models;

namespace Swagabond.Core.ObjectModel;

/// <summary>
/// The request body for the API.  Currently only one content type is supported and we assume that
/// the request body points to a schema definition.
/// </summary>
public class ApiRequestBody
{
    public bool IsEmpty { get;set; } = true;
    public string Description { get; set; } = string.Empty;
    public bool IsRequired { get; set; } = false;
    
    public ApiSchema Schema { get; set; } = new();
    public ApiContentType ContentType { get; set; } = ApiContentType.PlainText;
    public string RawContentType { get; set; } = string.Empty;

    public Api Api { get; set; } = new();

    /// <summary>
    /// When accessed from an operation, this will contain the operation that this request body is associated with.
    /// </summary>
    public ApiOperation Operation { get; set; }
    
    
    public static ApiRequestBody Empty = new();

    public static ApiRequestBody FromOpenApi(string name, OpenApiRequestBody requestBody, Api api, ApiOperation? apiOperation)
    {
        var apiRequestBody = new ApiRequestBody();
        
        apiRequestBody.Description = requestBody.Description ?? string.Empty;
        apiRequestBody.IsRequired = requestBody.Required;
        apiRequestBody.IsEmpty = false;
        apiRequestBody.Api = api;
        
        if (apiOperation is not null)
            apiRequestBody.Operation = apiOperation;
        
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
        
        apiRequestBody.Schema = ApiSchema.FromOpenApi(name, contentBody.Schema, api, apiOperation);
        
        return apiRequestBody;
    }
}