using Swagabond.ObjectModelV1.Interfaces;

namespace Swagabond.ObjectModelV1;

public class SchemaReferenceV1 : IObjectV1, INamedObject
{
    public bool IsEmpty { get; internal set; } = true;
    public string Name { get; internal set; } = string.Empty;
    public string Title { get; internal set; } = string.Empty;
    public string Description { get; internal set; } = string.Empty;
    
    /// <summary>
    /// The full / unfiltered name.  Useful in cases where the exact name
    /// must be used, including original casing.
    /// </summary>
    public string OriginalName { get; internal set; } = string.Empty;
    
    public SchemaDefinitionV1 Schema { get; internal set; } = SchemaDefinitionV1.Empty;

    public override string ToString()
    {
        return $"SchemaReferenceV1 {Name} ({Schema.Name})";
    }

    public static readonly SchemaReferenceV1 Empty = new();
}