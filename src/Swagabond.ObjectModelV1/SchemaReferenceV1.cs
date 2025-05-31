using Swagabond.ObjectModelV1.Interfaces;

namespace Swagabond.ObjectModelV1;

/// <summary>
/// a Name and reference to a schema definition. Used for named properties that
/// refer to a specific schema.
/// </summary>
public class SchemaReferenceV1 : IObjectV1, INamedObject
{
    /// <summary>
    /// If true, the schema reference is empty.
    /// </summary>
    public bool IsEmpty { get; internal set; } = true;
    /// <summary>
    /// The name of the property formatted as PascalCase.  Suitable for code / filenames.
    /// </summary>
    public string Name { get; internal set; } = string.Empty;
    /// <summary>
    /// Currently...this is always empty. 
    /// </summary>
    public string Title { get; internal set; } = string.Empty;
    
    /// <summary>
    /// A brief description of the schema reference.  
    /// </summary>
    public string Description { get; internal set; } = string.Empty;
    
    /// <summary>
    /// The full / unfiltered name.  Useful in cases where the exact name
    /// must be used, including original casing.  (this does not format as pascal case).
    /// /// There's no guarantee that this is suitable for filenames / code, so only use if you know
    /// /// how the schema names will look for your api spec.
    /// </summary>
    public string OriginalName { get; internal set; } = string.Empty;
    
    /// <summary>
    /// The schema definition that this reference points to. 
    /// </summary>
    public SchemaDefinitionV1 Schema { get; internal set; } = SchemaDefinitionV1.Empty;

    public override string ToString()
    {
        return $"SchemaReferenceV1 {Name} ({Schema.Name})";
    }

    public static readonly SchemaReferenceV1 Empty = new();
}