using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swagabond.ObjectModelV1.Extensions;

namespace Swagabond.ObjectModelV1.Transformer;

public interface ISchemaDefinitionV1Transformer
{
    SchemaDefinitionV1 FromOpenApi(OpenApiSchema schema, ApiV1 api);
}

public class SchemaDefinitionV1Transformer : ISchemaDefinitionV1Transformer
{
    private const string GuidExample = "f9a7b8c4-0c4f-46e1-b4f5-d2fe9e5b35bb";

    
    private IEnumOptionV1Transformer _enumOptionTransformer;
    private IDataTypeV1Transformer _dataTypeV1Transformer;
    private IExtensionV1Transformer _extensionV1Transformer;
    private ISchemaReferenceV1Transformer _schemaReferenceV1Transformer;

    public SchemaDefinitionV1Transformer(IEnumOptionV1Transformer enumOptionTransformer, IDataTypeV1Transformer dataTypeV1Transformer, 
        IExtensionV1Transformer extensionV1Transformer, ISchemaReferenceV1Transformer schemaReferenceV1Transformer)
    {
        _enumOptionTransformer = enumOptionTransformer;
        _dataTypeV1Transformer = dataTypeV1Transformer;
        _extensionV1Transformer = extensionV1Transformer;
        _schemaReferenceV1Transformer = schemaReferenceV1Transformer;
    }

    public SchemaDefinitionV1 FromOpenApi(OpenApiSchema schema, ApiV1 api)
    {
        var apiSchema = new SchemaDefinitionV1();
        // todo: extra properties 

        var isArray = schema.Type == "array";
        
        // Map as an enum
        if (schema.Enum.Any())
        {
            apiSchema.IsEnum = true;
            apiSchema.EnumOptions = _enumOptionTransformer.FromOpenApi(schema.Enum, schema.Extensions);
        }
        
        // Map the type 
        if (isArray)
        {
            if (schema.Items.Enum.Any())
            {
                apiSchema.IsEnum = true;
                apiSchema.EnumOptions = _enumOptionTransformer.FromOpenApi(schema.Items.Enum, schema.Items.Extensions);
            }

            apiSchema.IsArray = true;
            apiSchema.DataType = _dataTypeV1Transformer.FromOpenApi(schema.Items.Type, schema.Items.Format);
        }
        else
        {
            apiSchema.DataType = _dataTypeV1Transformer.FromOpenApi(schema.Type, schema.Format);
        }
        
        // If this is an object, map it's properties recursively
        if (apiSchema.DataType == DataTypeV1.Object)
        {
            foreach (var prop in schema.Properties)
            {
                // First recursively map each property 
                var propSchema = FromOpenApi(prop.Value, api);
                // then wrap in a reference with a name 
                apiSchema.Properties.Add(_schemaReferenceV1Transformer.FromOpenApi(prop.Key, propSchema, api ));
            }
        }
        
        // open api puts the interesting info in schema.items if this is an array
        var schemaToUse = isArray ? schema.Items : schema;
        
        var schemaId = schemaToUse.Reference?.Id;

        apiSchema.IsEmpty = false;
        apiSchema.Name = schemaId?.ToClassName() ?? apiSchema.DataType.ToString();
        apiSchema.OriginalName = schemaId ?? apiSchema.DataType.ToString();
        apiSchema.Title = schemaId ?? apiSchema.Name;
        apiSchema.Description = WriteDescription(schemaToUse.Description, apiSchema.DataType, apiSchema.IsArray, apiSchema.IsEnum);

        var example = schemaToUse.Example?.WriteAsString();

        apiSchema.ReferenceId = schemaToUse?.Reference?.Id ?? string.Empty;
        
        apiSchema.Example = string.IsNullOrEmpty(example) ? WriteGenericExample(apiSchema.DataType, apiSchema.EnumOptions) : example;
        // todo: confirm if this works vVv
        apiSchema.Extensions = _extensionV1Transformer.FromOpenApi(schema.Extensions);
        
        apiSchema.Api = api;

        return apiSchema;
    }


    // Clunkily build a description for the schema.
    // We don't try to come up with descriptions for enums or complex objects here, and we always return the custom description if one exists.
    private static string WriteDescription(string existingDescription, DataTypeV1 dataType, bool isArray, bool isEnum)
    {
        if (!string.IsNullOrWhiteSpace(existingDescription) || isEnum || dataType == DataTypeV1.Object)
            return existingDescription;

        var arrayChunk = isArray ? "An array of elements, where each item is " : string.Empty;
        
        var dataTypeChunk = dataType switch
        {
            DataTypeV1.String => "a sequence of characters, typically used for text data.",
            DataTypeV1.Int32 => "a 32-bit signed integer, used for whole numbers within the range of -2,147,483,648 to 2,147,483,647.",
            DataTypeV1.Int64 => "a 64-bit signed integer, used for larger whole numbers within the range of -9,223,372,036,854,775,808 to 9,223,372,036,854,775,807.",
            DataTypeV1.Guid => "a globally unique identifier, often used for unique keys or identifiers in systems.",
            DataTypeV1.DateTime => "a specific date and time, including both date and time components.",
            DataTypeV1.Boolean => "a true or false value, typically used for logical conditions.",
            DataTypeV1.Float => "a single-precision floating-point number, used for approximate representation of real numbers, with up to 7 decimal places.",
            DataTypeV1.Double => "a double-precision floating-point number, with up to 15-16 decimal places.",
            DataTypeV1.Decimal => "a high-precision decimal number, often used for financial or monetary calculations, with up to 28-29 decimal places.",
            _ => string.Empty
        };

        if (!isArray)
        {
            // captialize the first letter of the data type chunk
            dataTypeChunk = char.ToUpper(dataTypeChunk[0]) + dataTypeChunk[1..];
        }

        return $"{arrayChunk} {dataTypeChunk}";

    }

    private static string WriteGenericExample(DataTypeV1 apiSchemaDataType, List<EnumOptionV1> enumOptions) 
    {
        if (enumOptions.Any())
        {
            var first = enumOptions.First();
            return $"{first.Value} ({first.Name})";
        }
        
        return apiSchemaDataType switch
        {
            DataTypeV1.String => "Example String",
            DataTypeV1.Object => string.Empty,
            DataTypeV1.Int32 => "123",
            DataTypeV1.Int64 => "123456789",
            DataTypeV1.Guid => GuidExample,
            DataTypeV1.DateTime => new DateTime(1995, 3, 26).ToString("o"),
            DataTypeV1.Boolean => "true",
            DataTypeV1.Float => "3.1415927",
            DataTypeV1.Double => "3.141592653589793",
            DataTypeV1.Decimal => "3.1415926535897932384626433832795",
            _ => "Example Value"
        };
    }


    
}