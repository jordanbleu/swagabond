using Microsoft.OpenApi.Models;
using Swagabond.ObjectModelV1.Extensions;

namespace Swagabond.ObjectModelV1.Transformer;

public interface IRequestBodyV1Transformer
{
    RequestBodyV1 FromOpenApi(OpenApiRequestBody requestBody, OperationV1 operationV1, PathV1 pathV1, ApiV1 apiV1);
}

public class RequestBodyV1Transformer : IRequestBodyV1Transformer
{
    private ISchemaDefinitionV1Transformer _schemaDefinitionV1Transformer;
    

    public RequestBodyV1Transformer(ISchemaDefinitionV1Transformer schemaDefinitionV1Transformer)
    {
        _schemaDefinitionV1Transformer = schemaDefinitionV1Transformer;
    }


    public RequestBodyV1 FromOpenApi(OpenApiRequestBody requestBody, OperationV1 operationV1, PathV1 pathV1, ApiV1 apiV1)
    {
        var apiRequestBody = new RequestBodyV1();
        var content = requestBody.Content?.FirstOrDefault().Value ?? null;
        
        apiRequestBody.IsEmpty = false;
        apiRequestBody.Name = GetName(requestBody, pathV1, operationV1);
        apiRequestBody.Title = $"{operationV1.Method.ToUpperInvariant()} {pathV1.Route} RequestBody";
        apiRequestBody.Description = content?.Schema?.Description ?? string.Empty;;
        apiRequestBody.Api = apiV1;
        
        if (content is not null)
        {
            apiRequestBody.Schema = _schemaDefinitionV1Transformer.FromOpenApi(content.Schema!, apiV1);
        }

        return apiRequestBody;
    }
    
    private string GetName(OpenApiRequestBody requestBody, PathV1 path, OperationV1 operationV1)
    {
        return $"{path.Route.ToPascalCase().ToClassName()}{operationV1.Method.ToPascalCase().ToClassName()}Request";
    }
    
}