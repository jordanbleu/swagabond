using Microsoft.OpenApi.Models;
using Swagabond.ObjectModelV1.Extensions;

namespace Swagabond.ObjectModelV1.Transformer;

public interface IOperationV1Transformer
{
    OperationV1 FromOpenApi(KeyValuePair<OperationType, OpenApiOperation> operation, PathV1 path, ApiV1 api);
}

public class OperationV1Transformer : IOperationV1Transformer
{
    private ISchemaDefinitionV1Transformer _schemaDefinitionV1Transformer;
    private ISchemaReferenceV1Transformer _schemaReferenceV1Transformer;
    private IRequestBodyV1Transformer _requestBodyV1Transformer;
    private IResponseBodyV1Transformer _responseBodyV1Transformer;

    public OperationV1Transformer(ISchemaDefinitionV1Transformer schemaDefinitionV1Transformer, ISchemaReferenceV1Transformer schemaReferenceV1Transformer, IRequestBodyV1Transformer requestBodyV1Transformer, IResponseBodyV1Transformer responseBodyV1Transformer)
    {
        _schemaDefinitionV1Transformer = schemaDefinitionV1Transformer;
        _schemaReferenceV1Transformer = schemaReferenceV1Transformer;
        _requestBodyV1Transformer = requestBodyV1Transformer;
        _responseBodyV1Transformer = responseBodyV1Transformer;
    }


    public OperationV1 FromOpenApi(KeyValuePair<OperationType, OpenApiOperation> operation, PathV1 path, ApiV1 api)
    {
        var apiOperation = new OperationV1();

        var httpMethod = operation.Key.ToString();
        var o = operation.Value;

        apiOperation.IsEmpty = false;
        apiOperation.Name = $"{httpMethod.ToPascalCase()}{path.Name}";
        apiOperation.Title = $"{httpMethod.ToUpper()} {path.Route}";
        apiOperation.Description = o.Description;
        apiOperation.Method = httpMethod;

        foreach (var param in o.Parameters)
        {
            var schemaDef = _schemaDefinitionV1Transformer.FromOpenApi(param.Schema, api);
            var schemaRef = _schemaReferenceV1Transformer.FromOpenApi(param.Name, schemaDef, api);
            
            switch (param.In)
            {
                case ParameterLocation.Query:
                    apiOperation.QueryParameters.Add(schemaRef);
                    break;
                case ParameterLocation.Header:
                    apiOperation.HeaderParameters.Add(schemaRef);
                    break;
                case ParameterLocation.Path:
                    apiOperation.PathParameters.Add(schemaRef);
                    break;
                case ParameterLocation.Cookie:
                    apiOperation.CookieParameters.Add(schemaRef);
                    break;
            }
        }
        
        if (o.RequestBody is not null)
        {
            apiOperation.RequestBody = _requestBodyV1Transformer.FromOpenApi(o.RequestBody, apiOperation, path, api);
        }

        var responses = o.Responses;
        if (!responses.Any())
            return apiOperation;

        // Map the default response body
        var defaultResponse = responses.FirstOrDefault(kvp => kvp.Key == "default");
        if (!string.IsNullOrEmpty(defaultResponse.Key))
        {
            apiOperation.DefaultResponseBody = _responseBodyV1Transformer.FromOpenApi(defaultResponse, api, apiOperation);
        }
        
        // Map the other response bodies
        foreach (var response in responses)
        {
            if (response.Key == "default")
                continue;

            var responseBody = _responseBodyV1Transformer.FromOpenApi(response, api, apiOperation);
            apiOperation.ResponseBodies.Add(responseBody);
        }
        
        // If we still don't have a default response body, use the first success response body
        if (apiOperation.DefaultResponseBody.IsEmpty)
        {
            apiOperation.DefaultResponseBody = apiOperation.SuccessResponseBody;
        }

        apiOperation.Path = path;
        apiOperation.Api = api;
        return apiOperation;
    }


}