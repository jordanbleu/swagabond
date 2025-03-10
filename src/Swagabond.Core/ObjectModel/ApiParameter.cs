using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swagabond.Core.Constants;
using Swagabond.Core.Extensions;

namespace Swagabond.Core.ObjectModel;

/// <summary>
/// Query, Path, or Header parameters
/// </summary>
public class ApiParameter
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsRequired { get; set; } = false;
    public bool AllowEmptyValue { get; set; } = false;
    public bool IsArray { get; set; }
    public ApiDataType Type { get; set; } = ApiDataType.String;
    
    public bool IsEnum { get; set; } = false;
    public ICollection<string> EnumValues { get; set; } = Empty.Strings;
    public ICollection<string> EnumNames { get; set; } = Empty.Strings;
    public ICollection<ApiEnumOption> EnumOptions { get; set; } = new List<ApiEnumOption>();
    
    public string Format { get; set; } = string.Empty;

    public static ApiParameter FromOpenApi(OpenApiParameter parameter)
    {
        var apiParameter = new ApiParameter();
        
        apiParameter.Name = parameter.Name;
        apiParameter.Description = parameter.Description ?? string.Empty;
        apiParameter.IsRequired = parameter.Required;
        apiParameter.AllowEmptyValue = parameter.AllowEmptyValue;
        apiParameter.IsEnum = parameter.Schema.Enum?.Any() ?? false;

        if (apiParameter.IsEnum)
        {
            var schema = parameter.Schema;
            // Find the first matching supported enum names extension (if one exists)
            var enumNamesArrayKvp = schema.Extensions
                .Where(x => ExtensionConstants.EnumNamesExtension.Contains(x.Key))
                .FirstOrDefault(x=> x.Value is OpenApiArray arr && arr.Count == schema.Enum!.Count);

            var enumNamesArray = enumNamesArrayKvp.Value as OpenApiArray;
            
            // iterate through each enum value 
            for (var i=0; i<schema.Enum!.Count; i++)
            {
                var enumValue = schema.Enum[i].WriteAsString();
                
                if (enumNamesArray?.Any() == true)
                {
                    var enumName = enumNamesArray[i].WriteAsString();
                    apiParameter.EnumOptions.Add(new ApiEnumOption { Name = enumName, Value = enumValue });
                }
                else
                {
                    apiParameter.EnumOptions.Add(new ApiEnumOption { Name = $"Option{enumValue}", Value = enumValue });
                }
            }
        }
        
        var mainSchemaType = parameter.Schema.Type;
        
        if (mainSchemaType.ToLower() == "array")
        {
            apiParameter.IsArray = true;

            // For arrays we look at the 'items' to get the type
            var typeString = parameter.Schema.Items.Type;
            
            // No json or xml allowed in query params
            if (typeString.ToLower() == "object")
            {
                typeString = "string";
            }
            
            apiParameter.Type = ApiDataTypeMapper.FromString(typeString);
            apiParameter.Format = parameter.Schema.Items.Format ?? string.Empty;
        }
        else
        {
            var typeString =  parameter.Schema.Type;
            
            // No json or xml allowed in query params
            if (typeString.ToLower() == "object")
            {
                typeString = "string";
            }
            
            apiParameter.IsArray = false;
            apiParameter.Type = ApiDataTypeMapper.FromString(typeString);
            apiParameter.Format = parameter.Schema.Format ?? string.Empty;
        }

        return apiParameter;
    }
    
    
}