using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swagabond.Core.Constants;
using Swagabond.Core.Extensions;

namespace Swagabond.Core.ObjectModel;

public class ApiSchema
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
    
    public List<ApiSchema> Properties { get; set; } = new();

    public bool IsDeprecated { get; set; } = false;
    
    public string ReferenceSchemaId { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;

    public Api Api { get; set; } = new();

    public bool IsEmpty { get; set; } = true;

    public ApiOperation Operation { get; set; } = ApiOperation.Empty;

    public static ApiSchema FromOpenApi(string name, OpenApiSchema schema, Api fullApi, ApiOperation? apiOperation = null)
    {
        var apiSchema = new ApiSchema();

        apiSchema.Name = name;
        apiSchema.IsDeprecated = schema.Deprecated;
        apiSchema.IsEnum = schema.Enum?.Any() == true || schema.Items?.Enum?.Any() == true;
        apiSchema.Description = schema.Description ?? string.Empty;
        apiSchema.IsEmpty = false;
        apiSchema.Api = fullApi;
                
        var isArray =  schema.Type == "array";
        apiSchema.IsArray = isArray;

        if (apiOperation is not null)
            apiSchema.Operation = apiOperation;

        // Map enum values + names 
        if (apiSchema.IsEnum)
        {
            var enumToMap = apiSchema.IsArray ? schema.Items?.Enum : schema.Enum ?? schema.Enum;
            
            var enumOpts =  ApiEnumOption.FromOpenApi(enumToMap!, schema.Extensions);
            
            apiSchema.EnumOptions = enumOpts;
            apiSchema.EnumValues = enumOpts.Select(x=>x.Value).ToList();
            apiSchema.EnumNames = enumOpts.Select(x=>x.Name).ToList();
        }

        // set the example property to a string representation of the example value
        apiSchema.Example = schema.Example?.WriteAsString() ?? string.Empty;
        apiSchema.Format = schema.Format ?? string.Empty;


        // If it is an array we set the type to the type of array elements.
        // This is different from the OpenAPI spec because we don't support multi-type arrays.
        if (isArray)
        {
            apiSchema.Type = ApiDataTypeMapper.FromString(schema.Items.Type);
        }
        else
        {
            apiSchema.Type = ApiDataTypeMapper.FromString(schema.Type);
        }

        if (apiSchema.Type != ApiDataType.Object && !apiSchema.IsEnum) 
            return apiSchema;

        if (apiSchema.IsArray)
        {
            apiSchema.ReferenceSchemaId = schema.Items.Reference?.Id ?? string.Empty;

            foreach (var prop in schema.Items.Properties)
            {
                // recursively map each inner property
                apiSchema.Properties.Add(FromOpenApi(prop, fullApi));
            }
        }
        else
        {
            apiSchema.ReferenceSchemaId = schema.Reference?.Id ?? string.Empty;
                
            // If it is an object, map the inner properties as well
            foreach (var prop in schema.Properties)
            {
                // recursively map each inner property
                apiSchema.Properties.Add(FromOpenApi(prop, fullApi));
            }
        }
    
        return apiSchema;
    }

    public static ApiSchema FromOpenApi(KeyValuePair<string, OpenApiSchema> property, Api fullApi) =>
        FromOpenApi(property.Key, property.Value, fullApi);
}