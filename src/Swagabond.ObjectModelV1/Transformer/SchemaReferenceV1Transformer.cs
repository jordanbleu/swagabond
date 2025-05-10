using Microsoft.OpenApi.Models;

namespace Swagabond.ObjectModelV1.Transformer;

public interface ISchemaReferenceV1Transformer
{
    SchemaReferenceV1 FromOpenApi(string name, SchemaDefinitionV1 apiSchema, ApiV1 api);
}

public class SchemaReferenceV1Transformer : ISchemaReferenceV1Transformer
{
    public SchemaReferenceV1 FromOpenApi(string name, SchemaDefinitionV1 apiSchema, ApiV1 api)
    {
        var apiSchemaRef = new SchemaReferenceV1();

        apiSchemaRef.Name = name;
        apiSchemaRef.Schema = apiSchema;
        apiSchemaRef.IsEmpty = false;

        return apiSchemaRef;
    }   

}