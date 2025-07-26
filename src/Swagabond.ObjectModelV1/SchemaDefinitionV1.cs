using System.Dynamic;
using System.Text.Json;
using Swagabond.ObjectModelV1.Extensions;
using Swagabond.ObjectModelV1.Interfaces;

namespace Swagabond.ObjectModelV1;

/// <summary>
/// Defines the entire structure of a schema used in an API.
/// A SchemaDefinition can be a primitive type, complex object, enum, array, and more.
/// </summary>
public class SchemaDefinitionV1 : IObjectV1, INamedObject
{
    private static readonly JsonSerializerOptions ExampleSerializerSettings = new()
    {
        WriteIndented = true  
    };
    
    /// <summary>
    /// True if the schema definition is empty.
    /// </summary>
    public bool IsEmpty { get; internal set; } = true;
    
    /// <summary>
    /// A name for the schema definition, formatted as PascalCase. Suitable for code or filenames.
    /// </summary>
    public string Name { get; internal set; } = string.Empty;
    
    /// <summary>
    /// The full / unfiltered schema name.  Useful in cases where the exact name
    /// must be used, including original casing (this does not format as pascal case).
    /// There's no guarantee that this is suitable for filenames / code, so only use if you know
    /// how the schema names will look for your api spec.
    /// </summary>
    public string OriginalName { get; internal set; } = string.Empty;
    
    /// <summary>
    /// A more beautiful and gorgeous title for the schema definition. Not suitable for filenames, or code.
    /// </summary>
    public string Title { get; internal set; } = string.Empty;
    /// <summary>
    /// A brief description of the schema definition.
    /// </summary>
    public string Description { get; internal set; } = string.Empty;

    /// <summary>
    /// an example value for the object.  If the spec doesn't provide an example, a dummy
    /// value will be generated from Swagabond.  The example values won't always be actual
    /// valid values for the api. One thing to note is that a generic fallback example will NOT
    /// be provided for complex objects, so, instead you should use the JsonExample property.
    /// </summary>
    public object Example { get; internal set; } = string.Empty;

    private string? _jsonExample = null;
    public string JsonExample 
    {
        get
        {
            if (_jsonExample != null)
                return _jsonExample;

            var body = new Dictionary<string, object>();

            foreach (var prop in Properties)
            {
                if (prop.Schema.IsPrimitive)
                {
                    body.Add(prop.Name, prop.Schema.Example);
                    continue;
                }

                // This is weird and not efficient.
                // We are basically serializing the example and then deserializing it
                // many times over and over
                var jsonExample = prop.Schema.JsonExample;
                var parsedDoc = JsonDocument.Parse(jsonExample);
                body.Add(prop.Name, parsedDoc.RootElement.Clone());
            }

            _jsonExample = JsonSerializer.Serialize(body, ExampleSerializerSettings);
            return _jsonExample;
        }
    }

    /// <summary>
    /// If true, this schema defines an array of items rather than a single item.
    /// </summary>
    public bool IsArray { get; internal set; } = false;

    /// <summary>
    /// Whether this is an enum or not. An enum is a special type of schema that defines a set of named values.
    /// </summary>
    public bool IsEnum { get; internal set; } = false;
    
    /// <summary>
    /// For enums, this will contain a list of each enum name / value.  For non-enums, this will be
    /// empty.  Enum names are determined by adding an extension to the schema in your spec called
    /// x-enumNames, with each name corresponding to an enum value in the same order. If there's no
    /// enumNames extension, each enum name will be something dumb like Item1, Item2, etc.
    /// </summary>
    public List<EnumOptionV1> EnumOptions { get; internal set; } = new();
    
    /// <summary>
    /// This is the underlying type of the schema, whether it be an object or a primitive type.
    /// For enums, this represents the backing value of the enum (usually an int).
    /// </summary>
    public DataTypeV1 DataType { get; internal set; } = DataTypeV1.String;

    /// <summary>
    /// List of arbitrary extensions for this schema definition.
    /// </summary>
    public List<ExtensionV1> Extensions { get; internal set; } = new();
    
    private Dictionary<string, string>? _extensionDictionary = null;

    /// <summary>
    /// A dictionary of extensions where the key is the extension name and the value
    /// is its value.  This allows you to bind directly to known keys instead of iterating
    /// over the list of extensions. Values can be accessed via `ExtensionDictionary["x-myKey"]`
    /// </summary>
    public Dictionary<string, string> ExtensionDictionary {
        get
        {
            if (_extensionDictionary == null)
            {
                _extensionDictionary = Extensions.ToDictionary(e => e.Name, e => e.Value);
            }

            return _extensionDictionary;
        }
    }

    /// <summary>
    /// The API that this belongs to
    /// </summary>
    public ApiV1 Api { get; internal set; } = ApiV1.Empty;

    /// <summary>
    /// For a complex object type, this contains a list of inner properties for this schema definition.
    /// Each item in this list contains the property name and its schema definition.
    /// </summary>
    public List<SchemaReferenceV1> Properties { get; set; } = new();
    
    /// <summary>
    /// An identifier for this schema item.  For OpenAPI, this will point to the schema reference id.
    /// </summary>
    public string ReferenceId { get; internal set; } = string.Empty;

    /// <summary>
    /// A set of validation rules around the schema, generally used for client side
    /// validation.
    /// </summary>
    public SchemaConstraintsV1 Constraints { get; set; } = SchemaConstraintsV1.Empty;

    /// <summary>
    /// Returns true if the schema type is a simple value (not a complex object)
    /// </summary>
    public bool IsPrimitive
    {
        get => DataType != DataTypeV1.Object;
    }


    public override string ToString()
    {
        return $"SchemaDefinition {Name} {DataType}";
    }

    public static readonly SchemaDefinitionV1 Empty = new ();
}