using Microsoft.OpenApi.Models;

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
    
    public ApiDataType Type { get; set; } = ApiDataType.String;
    
    public string Format { get; set; } = string.Empty;

    public static ApiParameter FromOpenApi(OpenApiParameter parameter)
    {
        // todo: need to handle arrays 
        var apiParameter = new ApiParameter();
        
        apiParameter.Name = parameter.Name;
        apiParameter.Description = parameter.Description ?? string.Empty;
        apiParameter.IsRequired = parameter.Required;
        apiParameter.AllowEmptyValue = parameter.AllowEmptyValue;

        var typeString = parameter.Schema.Type;

        // I refuse to support json in these parameters.
        if (typeString.ToLower() == "object")
        {
            typeString = "string";
        }
        
        apiParameter.Type = ApiDataTypeMapper.FromString(typeString);
        apiParameter.Format = parameter.Schema.Format ?? string.Empty;

        return apiParameter;
    }
    
    
}