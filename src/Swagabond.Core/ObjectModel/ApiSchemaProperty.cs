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

    public ICollection<string> EnumValues { get; set; } = Empty.Strings;

    public ICollection<string> EnumNames { get; set; } = Empty.Strings;
    
    public ICollection<ApiEnumOption> EnumOptions { get; set; } = new List<ApiEnumOption>();
    
    public List<ApiSchemaProperty> Properties { get; set; } = new();
    
    public static ApiSchemaProperty FromOpenApi(KeyValuePair<string, OpenApiSchema> property)
    {
        
        var apiProperty = new ApiSchemaProperty();
        var p = property.Value;
        
        apiProperty.Name = property.Key;
        
        apiProperty.IsEnum = p.Enum?.Any() ?? false;

        // Map enum values + names 
        if (apiProperty.IsEnum)
        {
            // Find the first matching supported enum names extension (if one exists)
            var enumNamesArrayKvp = p.Extensions
                .Where(x => ExtensionConstants.EnumNamesExtension.Contains(x.Key))
                .FirstOrDefault(x=> x.Value is OpenApiArray arr && arr.Count == p.Enum.Count);
            
            var enumNamesArray = enumNamesArrayKvp.Value as OpenApiArray;
            
            // iterate through each enum value 
            for (var i=0; i<p.Enum!.Count; i++)
            {
                var enumValue = p.Enum[i].WriteAsString();
                
                if (enumNamesArray?.Any() == true)
                {
                    var enumName = enumNamesArray[i].WriteAsString();
                    apiProperty.EnumOptions.Add(new ApiEnumOption { Name = enumName, Value = enumValue });
                }
                else
                {
                    apiProperty.EnumOptions.Add(new ApiEnumOption { Name = $"Option{enumValue}", Value = enumValue });
                }
            }
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