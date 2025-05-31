using Swagabond.ObjectModelV1.Interfaces;

namespace Swagabond.ObjectModelV1;

/// <summary>
/// Defines the entire structure of a schema used in an API.
/// A SchemaDefinition can be a primitive type, complex object, enum, array, and more.
/// </summary>
public class SchemaDefinitionV1 : IObjectV1, INamedObject
{
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
    /// valid values for the api.
    /// </summary>
    public string Example { get; internal set; } = string.Empty;

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
    /// If true this schema definition is a primitive object (string, int, etc), including enums.
    /// If false, this is a complex object with inner properties.
    /// </summary>
    public bool IsPrimitive { get => DataType!=DataTypeV1.Object; }

    public override string ToString()
    {
        return $"SchemaDefinition {Name} {DataType}";
    }

    public static readonly SchemaDefinitionV1 Empty = new ();
}