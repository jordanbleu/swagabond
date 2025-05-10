using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Swagabond.ObjectModelV1.Extensions;

namespace Swagabond.ObjectModelV1.Transformer;

public interface IEnumOptionV1Transformer
{
    List<EnumOptionV1> FromOpenApi(IList<IOpenApiAny> enumValues, IDictionary<string, IOpenApiExtension> extensions);
}

public class EnumOptionV1Transformer : IEnumOptionV1Transformer
{
    /// <summary>
    /// This is a list of extension names we will check to find enum names.
    ///
    /// OpenAPI doesn't natively support enums as key-value pairs, so we have to use extensions.
    /// https://stackoverflow.com/questions/66465888/how-to-define-enum-mapping-in-openapi
    ///
    /// The only supported extensions we recognize need to be just a basic list of strings, and the list
    /// of strings needs to be the same length as the enum values since we match them index by index.
    /// </summary>
    private static readonly ICollection<string> EnumNamesExtensionKeys = new List<string> 
    {
        "x-enumNames",
        "x-enum-varnames",
        "x-speakeasy-enums"
    };
    
    public List<EnumOptionV1> FromOpenApi(IList<IOpenApiAny> enumValues, IDictionary<string, IOpenApiExtension> extensions)
    {
        if (!enumValues.Any())
            return new();
        
        // To find a valid set of enum names:
        // First find an extension that has any of our supported enum name keys
        // Then check if it has a length equal to the length of enum values
        
        var enumNamesArrayKvp = extensions
            .Where(x => EnumNamesExtensionKeys.Contains(x.Key))
            .FirstOrDefault(x=> x.Value is OpenApiArray arr && arr.Count == enumValues.Count);

        var enumNamesArray = enumNamesArrayKvp.Value as OpenApiArray;

        var enumOptions = new List<EnumOptionV1>();
        
        for (var i = 0; i < enumValues.Count; i++)
        {
            var enumValue = enumValues[i].WriteAsString();
            var enumName = enumNamesArray?[i].WriteAsString() ?? ("Item" + i);
            
            var option = new EnumOptionV1
            {
                Name = enumName,
                Value = enumValue
            };
            enumOptions.Add(option);
        }

        return enumOptions;
    }    
}