using Microsoft.OpenApi.Models;
using Swagabond.ObjectModelV1.Extensions;

namespace Swagabond.ObjectModelV1.Transformer;

public interface IResponseBodyV1Transformer
{
    ResponseBodyV1 FromOpenApi(KeyValuePair<string, OpenApiResponse> response, ApiV1 api, OperationV1 operation);
}

public class ResponseBodyV1Transformer : IResponseBodyV1Transformer
{
    
    private ISchemaDefinitionV1Transformer _schemaDefinitionV1Transformer;

    public ResponseBodyV1Transformer(ISchemaDefinitionV1Transformer schemaDefinitionV1Transformer)
    {
        _schemaDefinitionV1Transformer = schemaDefinitionV1Transformer;
    }

    public ResponseBodyV1 FromOpenApi(KeyValuePair<string, OpenApiResponse> response, ApiV1 api, OperationV1 operation)
    {
        var apiResponse = new ResponseBodyV1();

        var r = response.Value;
        var id = response.Key;
        
        var name = $"{operation.Path.Name.ToPascalCase()}{response.Key.ToPascalCase()}{operation.Method.ToPascalCase()}Response";

        apiResponse.IsEmpty = false;
        apiResponse.Name = name;
        apiResponse.Title = $"{id} Response - {name}";
        apiResponse.Description = r.Description;

        apiResponse.ResponseId = id;
        
        // try to add the status code if we can
        if (int.TryParse(id, out var statusCode))
            apiResponse.StatusCode = statusCode;

        apiResponse.Api = api;
        apiResponse.Operation = operation;

        var content = r.Content?.FirstOrDefault().Value ?? null;

        if (content is null)
            return apiResponse;

        apiResponse.Schema = _schemaDefinitionV1Transformer.FromOpenApi(content.Schema, api);

        return apiResponse;
    }
}
