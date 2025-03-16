using Microsoft.OpenApi.Models;

namespace Swagabond.Core.ObjectModel;

public class ApiResponseBody
{
    public bool IsEmpty { get; set; } = true;
    public string StatusCode { get; set; } = "200";
    public int ParsedStatusCode { get; set; } = 0;
    
    public string RawContentType { get; set; } = string.Empty;
    public ApiContentType ContentType { get; set; } = ApiContentType.PlainText;
    
    public string Description { get; set; } = string.Empty;
    
    public ApiSchema Schema { get; set; } = new();
    public Api Api { get; set; } = new();

    public ApiOperation Operation { get; set; } = new();
    public static ApiResponseBody Empty = new();


    public static ApiResponseBody FromOpenApi(string name, KeyValuePair<string, OpenApiResponse> response, Api api, ApiOperation operation)
    {
        var apiResponse = new ApiResponseBody();
        var r = response.Value;

        var statusCode = response.Key;
        apiResponse.StatusCode = statusCode;
        apiResponse.Description = r.Description;
        apiResponse.IsEmpty = false;
        apiResponse.Api = api;
        apiResponse.Operation = operation;
        
        if (int.TryParse(statusCode, out var parsedStatusCode))
            apiResponse.ParsedStatusCode = parsedStatusCode;
        
        var contentKvpMaybe = r.Content?.FirstOrDefault();
        
        if (r.Content?.Any() == false || contentKvpMaybe is null)
            return apiResponse;
        
        var contentKvp = contentKvpMaybe.Value;
        var rawContentType = contentKvp.Key;
        var content = contentKvp.Value;
        
        apiResponse.RawContentType = rawContentType;
        apiResponse.ContentType = ApiContentTypeMapper.FromString(rawContentType);
        
        
        apiResponse.Schema = ApiSchema.FromOpenApi(name, content.Schema, api, operation);
        return apiResponse;
    }
}