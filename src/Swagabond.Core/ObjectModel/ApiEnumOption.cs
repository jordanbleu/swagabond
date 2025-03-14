using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Swagabond.Core.Constants;
using Swagabond.Core.Extensions;

namespace Swagabond.Core.ObjectModel;

public class ApiEnumOption
{
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    
    public static List<ApiEnumOption> FromOpenApi(IList<IOpenApiAny> enumValues, IDictionary<string, IOpenApiExtension> extensions)
    {
        if (!enumValues.Any())
            return new();
        
        // To find a valid set of enum names:
        // First find an extension that has any of our supported enum name keys
        // Then check if it has a length equal to the length of enum values
        
        var enumNamesArrayKvp = extensions
            .Where(x => ExtensionConstants.EnumNamesExtensionKeys.Contains(x.Key))
            .FirstOrDefault(x=> x.Value is OpenApiArray arr && arr.Count == enumValues.Count);

        var enumNamesArray = enumNamesArrayKvp.Value as OpenApiArray;

        var enumOptions = new List<ApiEnumOption>();
        
        for (var i = 0; i < enumValues.Count; i++)
        {
            var enumValue = enumValues[i].WriteAsString();
        
            var enumName = enumNamesArray?[i].WriteAsString() ?? "Item" + i;
            
            var option = new ApiEnumOption
            {
                Name = enumName,
                Value = enumValue
            };
            enumOptions.Add(option);
        }

        return enumOptions;
    }
}