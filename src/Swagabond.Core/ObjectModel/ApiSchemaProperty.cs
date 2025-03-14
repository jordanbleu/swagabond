using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swagabond.Core.Constants;
using Swagabond.Core.Extensions;

namespace Swagabond.Core.ObjectModel;

public class ApiSchemaProperty
{
    public string Name { get; set; } = string.Empty;
    
    public string Example { get; set; } = string.Empty;
    
    public ApiDataType Type { get; set; } = ApiDataType.String;
    
    public string Format { get; set; } = string.Empty;
    
    public bool IsArray { get; set; } = false;

    public bool IsEnum { get; set; } = false; 
    
    public bool IsPrimitive => Type != ApiDataType.Object;

    public ICollection<string> EnumValues { get; set; } =  new List<string>();

    public ICollection<string> EnumNames { get; set; } =  new List<string>();
    
    public ICollection<ApiEnumOption> EnumOptions { get; set; } = new List<ApiEnumOption>();
    
    public List<ApiSchemaProperty> Properties { get; set; } = new();

    public string SchemaId { get; set; } = string.Empty;


    public static ApiSchemaProperty FromOpenApi(KeyValuePair<string, OpenApiSchema> property)
    {
        
        var apiProperty = new ApiSchemaProperty();
        var p = property.Value;
        
        apiProperty.Name = property.Key;
        
        apiProperty.IsEnum = p.Enum?.Any() ?? false;

        // Map enum values + names 
        if (apiProperty.IsEnum)
        {
            var enumOpts =  ApiEnumOption.FromOpenApi(p.Enum!, p.Extensions);
            apiProperty.EnumOptions = enumOpts;
            apiProperty.EnumValues = enumOpts.Select(x=>x.Value).ToList();
            apiProperty.EnumNames = enumOpts.Select(x=>x.Name).ToList();
        }

        // set the example property to a string representation of the example value
        apiProperty.Example = p.Example?.WriteAsString() ?? string.Empty;
        apiProperty.Format = p.Format ?? string.Empty;
        
        var isArray =  p.Type == "array";
        apiProperty.IsArray = isArray;

        // If it is an array we set the type to the type of array elements.
        // This is different from the OpenAPI spec because we don't support multi-type arrays.
        if (isArray)
        {
            apiProperty.Type = ApiDataTypeMapper.FromString(p.Items.Type);
        }
        else
        {
            apiProperty.Type = ApiDataTypeMapper.FromString(p.Type);
        }

        if (apiProperty.Type != ApiDataType.Object) 
            return apiProperty;

        if (apiProperty.IsArray)
        {
            apiProperty.SchemaId = p.Items.Reference.Id ?? string.Empty;

            foreach (var prop in p.Items.Properties)
            {
                // recursively map each inner property
                apiProperty.Properties.Add(FromOpenApi(prop));
            }
        }
        else
        {
            apiProperty.SchemaId = p.Reference.Id ?? string.Empty;
                
            // If it is an object, map the inner properties as well
            foreach (var prop in p.Properties)
            {
                // recursively map each inner property
                apiProperty.Properties.Add(FromOpenApi(prop));
            }
        }


            
        return apiProperty;
        
    }
}