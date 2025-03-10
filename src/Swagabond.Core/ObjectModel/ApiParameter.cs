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

    public bool IsArray { get; set; }
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

        apiParameter.Format = parameter.Schema.Format ?? string.Empty;
        
        if (typeString.ToLower() == "array")
        {
            apiParameter.IsArray = true;
            typeString = parameter.Schema.Items.Type;
            apiParameter.Format = parameter.Schema.Items.Format ?? string.Empty;
        }


        apiParameter.Type = ApiDataTypeMapper.FromString(typeString);

        return apiParameter;
    }
    
    
}