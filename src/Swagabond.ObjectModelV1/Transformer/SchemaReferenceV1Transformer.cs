using Microsoft.OpenApi.Models;
using Swagabond.ObjectModelV1.Extensions;

namespace Swagabond.ObjectModelV1.Transformer;

public interface ISchemaReferenceV1Transformer
{
    SchemaReferenceV1 FromOpenApi(string name, SchemaDefinitionV1 apiSchema, ApiV1 api);
}

public class SchemaReferenceV1Transformer : ISchemaReferenceV1Transformer
{
    public SchemaReferenceV1 FromOpenApi(string unfilteredName, SchemaDefinitionV1 apiSchema, ApiV1 api)
    {
        var apiSchemaRef = new SchemaReferenceV1();

        var cleanName = unfilteredName.ToClassName();
        
        apiSchemaRef.Name = cleanName;
        apiSchemaRef.OriginalName = unfilteredName;
        
        apiSchemaRef.Schema = apiSchema;
        apiSchemaRef.IsEmpty = false;

        return apiSchemaRef;
    }   

}