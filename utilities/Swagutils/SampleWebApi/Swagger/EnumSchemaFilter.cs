using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SampleWebApi.Swagger;

public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (!context.Type.IsEnum) 
            return;

        var enumNames = new OpenApiArray();

        // Gets the underlying type of the enum (int, byte, long, short ,etc)
        var underlyingType = Enum.GetUnderlyingType(context.Type) ?? typeof(int);
        
        var schemaType = string.Empty;
        var schemaFormat = string.Empty;
        
        if (underlyingType == typeof(long))
        {
            schemaType = "integer";
            schemaFormat = "int64";
        }
        else if (underlyingType == typeof(byte))
        {
            schemaType = "integer";
            schemaFormat = "byte";
        }
        else if (underlyingType == typeof(short))
        {
            schemaType = "integer";
            schemaFormat = "int16";
        }
        else
        {
            schemaType = "integer";
            schemaFormat = null;
        }
        
        foreach (var name in Enum.GetNames(context.Type))
        {
            enumNames.Add(new OpenApiString(name));
        }
        
        // Per this post, could be under a variety of names: x-enum-varnames, etc
        // https://stackoverflow.com/questions/66465888/how-to-define-enum-mapping-in-openapi 
        schema.Extensions.Add("x-enumNames", enumNames);
    }
}