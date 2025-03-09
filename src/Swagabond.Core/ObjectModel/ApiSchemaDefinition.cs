using Microsoft.OpenApi.Models;

namespace Swagabond.Core.ObjectModel;

public class ApiSchemaDefinition
{
    public string Title = string.Empty;
    public string SchemaId = string.Empty;
    public List<ApiSchemaProperty> Properties { get; set; } = new();
    
    public static ApiSchemaDefinition FromOpenApi(OpenApiSchema schema)
    {
        var apiSchema = new ApiSchemaDefinition();
        apiSchema.Title = schema.Title ?? String.Empty;
        apiSchema.SchemaId = schema.Reference?.Id ?? string.Empty;;
        
        foreach (var property in schema.Properties)
        {
            apiSchema.Properties.Add(ApiSchemaProperty.FromOpenApi(property));
        }
        
        return apiSchema;
    }
    
}