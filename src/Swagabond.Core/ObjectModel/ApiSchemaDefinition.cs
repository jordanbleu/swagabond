using Microsoft.OpenApi.Models;
using Swagabond.Core.Extensions;

namespace Swagabond.Core.ObjectModel;

public class ApiSchemaDefinition
{
    public string Format { get; set; } = string.Empty;
    public bool IsArray { get; set; } = false;
    public ApiDataType Type { get; set; } = ApiDataType.String;
    public bool IsEnum { get; set; } = false;
    public ICollection<string> EnumValues { get; set; } =  new List<string>();
    public ICollection<string> EnumNames { get; set; } = new List<string>();
    public ICollection<ApiEnumOption> EnumOptions { get; set; } = new List<ApiEnumOption>();
    public bool IsPrimitive => Type != ApiDataType.Object;
    
    public string Name => string.IsNullOrEmpty(SchemaId) ? SchemaId : Title;

    public string Title { get; set; } = string.Empty;
    public string SchemaId { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Example { get; set; } = string.Empty;
    public bool IsDeprecated { get; set; } = false;
    public List<ApiSchemaProperty> Properties { get; set; } = new();
    
    public static ApiSchemaDefinition FromOpenApi(OpenApiSchema schema)
    {
        var apiSchema = new ApiSchemaDefinition();
        apiSchema.Title = schema.Title ?? String.Empty;
        apiSchema.SchemaId = schema.Reference?.Id ?? string.Empty;
        apiSchema.Description = schema.Description ?? string.Empty;
        apiSchema.Example = schema.Example?.WriteAsString() ?? string.Empty;
        apiSchema.IsDeprecated = schema.Deprecated;
        
        foreach (var property in schema.Properties)
        {
            apiSchema.Properties.Add(ApiSchemaProperty.FromOpenApi(property));
        }

        var format = schema.Format;
        var schemaType = schema.Type;
        
        // Mapping based on the items of the array
        if (schemaType.ToLower() == "array")
        {
            apiSchema.IsArray = true;
            schemaType = schema.Items?.Type;
            format = schema.Items?.Format;
        }

        apiSchema.IsEnum = schema.Enum?.Any() ?? false;
        
        if (apiSchema.IsEnum)
        {
            var enumOpts =  ApiEnumOption.FromOpenApi(schema.Enum!, schema.Extensions);
            apiSchema.EnumOptions = enumOpts;
            apiSchema.EnumValues = enumOpts.Select(x=>x.Value).ToList();
            apiSchema.EnumNames = enumOpts.Select(x=>x.Name).ToList();
        }
        
        apiSchema.Type = ApiDataTypeMapper.FromString(schemaType);
        apiSchema.Format = format ?? string.Empty;
        
        return apiSchema;
    }
    
}