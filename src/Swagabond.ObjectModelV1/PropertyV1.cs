using Swagabond.ObjectModelV1.Interfaces;

namespace Swagabond.ObjectModelV1;

[Obsolete("don't use this i dont think we need it")]
public class PropertyV1 : IObjectV1, INamedObject
{
    public bool IsEmpty { get; internal set; } = true;
    public string Name { get; internal set; } = string.Empty;
    public string Title { get; internal set; } = string.Empty;
    public string Description { get; internal set; } = string.Empty;

    /// <summary>
    /// Whether or not this property is required. This means the client
    /// is required to pass this value with every request.
    /// Can be used to validate the request property before sending to the server.
    /// </summary>
    public bool IsRequired { get; internal set; } = false;

    /// <summary>
    /// If true, this means the property can have the name specified but no value.
    /// For example, a query parameter can be specified as `?foo=` but not `?foo=bar`
    /// Note this is different from the 'Nullable' and 'IsRequired' properties.
    /// </summary>
    public bool AllowEmptyValue { get; internal set; } = false;

    /// <summary>
    /// If true, the property's value can be 'null'.
    /// Note, this is different than `AllowEmptyValue`
    /// </summary>
    public bool IsNullable { get; internal set; } = false;

    /// <summary>
    /// For primitive typed properties (string, int, etc), this will be the data type.
    /// For anything else, this will be 'object'.
    /// </summary>
    public DataTypeV1 Type { get; internal set; } = DataTypeV1.String;

    /// <summary>
    /// If true, this should be treated as an array or collection
    /// </summary>
    public bool IsArray { get; internal set; } = false;

    /// <summary>
    /// If true, the 
    /// </summary>
    public bool IsEnum { get; internal set; } = false;
    
    public SchemaReferenceV1 Schema { get; internal set; } = SchemaReferenceV1.Empty;

    public static readonly PropertyV1 Empty = new();
}