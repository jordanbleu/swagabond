using Microsoft.OpenApi.Models;

namespace Swagabond.Core.ObjectModel;

public class ApiOperation
{
    /// <summary>
    /// For HTTP requests, this will be the HTTP Method.
    /// </summary>
    public string Method { get; set; } = string.Empty;
    
    /// <summary>
    ///  Tags used for grouping operations
    /// </summary>
    public List<ApiTag> Tags { get; set; } = new();
    
    public string Summary { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public string OperationId { get; set; } = string.Empty;
    
    /// <summary>
    /// For operations that accept a request body, this will contain a
    /// reference to the schema definition for the request body.
    /// </summary>
    public ApiRequestBody RequestBody { get; set; } = new();
    public List<ApiResponseBody> Responses { get; set; } = new();
    public List<ApiParameter> QueryParameters { get; set; } = new();
    
    public List<ApiParameter> HeaderParameters { get; set; } = new();

    public List<ApiParameter> PathParameters { get; set; } = new();

    /// <summary>
    /// If a success response is defined, this will return the first success response (http status less than 300). 
    /// </summary>
    private ApiResponseBody? _successResponse = null;
    
    public ApiResponseBody SuccessResponse 
    {
        get 
        {
            if (_successResponse is not null)
                return _successResponse;

            var first200Response = Responses.FirstOrDefault(r=>r.ParsedStatusCode is > 199 and < 300);
            
            if (first200Response is not null)
            {
                _successResponse = first200Response;
                return _successResponse;
            }
            
            var defaultResponse = Responses.FirstOrDefault(r=>r.StatusCode == "default");
            
            if (defaultResponse is not null)
            {
                _successResponse = defaultResponse;
                return _successResponse;
            }

            _successResponse = new();
            return _successResponse;
        }
    }

    private ApiResponseBody? _errorResponse = null;
    
    public ApiResponseBody ErrorResponse 
    {
        get 
        {
            if (_errorResponse is not null)
                return _errorResponse;

            // If an error response is defined, pick the first one
            var first400Response = Responses.FirstOrDefault(r=>r.ParsedStatusCode > 399);
            
            if (first400Response is not null)
            {
                _errorResponse = first400Response;
                return _errorResponse;
            }
            
            // if a default response is defined, use that
            var defaultResponse = Responses.FirstOrDefault(r=>r.StatusCode == "default");

            if (defaultResponse is not null)
            {
                _errorResponse = defaultResponse;
                return _errorResponse;
            }

            // if not responses are defined then just populate an empty one.
            _errorResponse = new();
            return _errorResponse;
        
        }
    }

    /// <summary>
    /// The path that this operation belongs to
    /// </summary>
    public ApiPath? Path { get; set; }



// todo: Security definition
    // todo: extensions 
// todo: Tags

    public static ApiOperation FromOpenApi(KeyValuePair<OperationType, OpenApiOperation> operation, OpenApiDocument api, ApiPath path)
    {
        var apiOperation = new ApiOperation();
        apiOperation.Path = path;
        
        var method = operation.Key.ToString();
        var op = operation.Value;

        apiOperation.Method = method;
        apiOperation.Summary = op.Summary ?? string.Empty;
        apiOperation.Description = op.Description ?? string.Empty;
        apiOperation.OperationId = op.OperationId ?? string.Empty;
        
        // Request body is (generally) a complex json-based object
        if (op.RequestBody is not null)
            apiOperation.RequestBody = ApiRequestBody.FromOpenApi(op.RequestBody);

        // Map basic parameters as well 
        foreach (var param in op.Parameters)
        {
            // * Cookie parameters are not supported
            switch (param.In)
            {
                case ParameterLocation.Query:
                    apiOperation.QueryParameters.Add(ApiParameter.FromOpenApi(param));
                    break;
                case ParameterLocation.Header:
                    apiOperation.HeaderParameters.Add(ApiParameter.FromOpenApi(param));
                    break;
                case ParameterLocation.Path:
                    apiOperation.PathParameters.Add(ApiParameter.FromOpenApi(param));
                    break;
            }
        }
        
        // Add a response body for each http status code
        foreach (var response in op.Responses)
        {
            apiOperation.Responses.Add(ApiResponseBody.FromOpenApi(response));
        }
        
        return apiOperation;
    }
}