using Swagabond.ObjectModelV1.Interfaces;

namespace Swagabond.ObjectModelV1;

// Note - can be a class or enum

public class SchemaDefinitionV1 : IObjectV1, INamedObject
{
    public bool IsEmpty { get; internal set; } = true;
    public string Name { get; internal set; } = string.Empty;
    
    /// <summary>
    /// The full / unfiltered name.  Useful in cases where the exact name
    /// must be used, including original casing.
    /// </summary>
    public string OriginalName { get; internal set; } = string.Empty;
    
    public string Title { get; internal set; } = string.Empty;
    public string Description { get; internal set; } = string.Empty;

    /// <summary>
    /// an example value for the object
    /// </summary>
    public string Example { get; internal set; } = string.Empty;

    /// <summary>
    /// If true, this schema defines an array of items rather than a single item.
    /// </summary>
    public bool IsArray { get; internal set; } = false;

    /// <summary>
    /// whether this is an enum or not.
    /// </summary>
    public bool IsEnum { get; internal set; } = false;
    /// <summary>
    /// For enums, this will be a list of each name / value
    /// </summary>
    public List<EnumOptionV1> EnumOptions { get; internal set; } = new();
    /// <summary>
    /// For enums, this will be the underlying type of the schema enum.
    /// </summary>
    public DataTypeV1 DataType { get; internal set; } = DataTypeV1.String;

    public List<ExtensionV1> Extensions { get; internal set; } = new();

    public ApiV1 Api { get; internal set; } = ApiV1.Empty;

    public List<SchemaReferenceV1> Properties { get; set; } = new();
    
    /// <summary>
    /// An identifier for this schema item.  For OpenAPI, this will point to the schema reference id.
    /// </summary>
    public string ReferenceId { get; internal set; } = string.Empty;

    /// <summary>
    /// If true this schema definition is a primitive object (string, int, etc)
    /// </summary>
    public bool IsPrimitive { get => DataType!=DataTypeV1.Object; }

    public override string ToString()
    {
        return $"SchemaDefinition {Name} {DataType}";
    }

    public static readonly SchemaDefinitionV1 Empty = new ();
}