using Microsoft.OpenApi.Models;
using Swagabond.Core.Constants;

namespace Swagabond.Core.ObjectModel;

public class ApiSchemaProperty
{
    public string Name { get; set; } = string.Empty;
    
    public string Example { get; set; } = string.Empty;
    
    public ApiDataType Type { get; set; } = ApiDataType.String;
    
    public string Format { get; set; } = string.Empty;
    
    public bool IsArray { get; set; } = false;

    public bool IsEnum { get; set; } = false; 

    public ICollection<string> EnumValues { get; set; } = Empty.Strings;

    public ICollection<string> EnumNames { get; set; } = Empty.Strings;
    
    public List<ApiSchemaProperty> Properties { get; set; } = new();
    
    public static ApiSchemaProperty FromOpenApi(KeyValuePair<string, OpenApiSchema> property)
    {
        // todo: map enums to a KVP list
        
        var apiProperty = new ApiSchemaProperty();
        var p = property.Value;
        
        apiProperty.Name = property.Key;
        
        // set the example property to a string representation of the example value
        apiProperty.Example = p.Example?.ToString() ?? string.Empty; // todo: this does not work.
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

        if (p.Type != "object") 
            return apiProperty;
            
        // If it is an object, map the inner properties as well
        foreach (var prop in p.Properties)
        {
            // recursively map each inner property
            apiProperty.Properties.Add(FromOpenApi(prop));
        }
            
        return apiProperty;


    }
}