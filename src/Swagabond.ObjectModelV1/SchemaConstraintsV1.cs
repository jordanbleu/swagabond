using Swagabond.ObjectModelV1.Interfaces;

namespace Swagabond.ObjectModelV1;

/// <summary>
/// List of validation rules around a property 
/// </summary>
public class SchemaConstraintsV1 : IObjectV1
{
    public static SchemaConstraintsV1 Empty = new();
    /// <summary>
    /// Whether there are any constraints specified on this property at all
    /// </summary>
    public bool IsEmpty { get; set; } = true;

    /// <summary>
    /// For numeric types, this will be true if the property specifies a minimum
    /// value, which is indicated by the MinValue property.
    /// </summary>
    public bool HasMinValue { get; set; } = false;

    /// <summary>
    /// For numeric types, this will be the minimum value allowed. Whether this is
    /// inclusive or exclusive depends on the "IsMinValueInclusive" property.
    /// Before using this value, you may want to check the `HasMinValue` property
    /// which specifies if a min value check is needed.
    /// </summary>
    /// <remarks>Check HasMinValue before using this</remarks>
    public decimal MinValue { get; set; } = decimal.MinValue;

    /// <summary>
    /// If true, the value of MinValue itself is allowed for this value.  So
    /// `value >= MinValue`. If false, MinValue itself is NOT allowed, so `value > MinValue`
    /// </summary>
    public bool IsMinValueInclusive { get; set; } = true;
    
    /// <summary>
    /// For numeric types, this will be true if the property specifies a maximum
    /// value, which is indicated by the MaxValue property.
    /// </summary>
    public bool HasMaxValue { get; set; } = false;

    /// <summary>
    /// For numeric types, this will be the max value allowed. Whether this is
    /// inclusive or exclusive depends on the "IsMaxValueInclusive" property.
    /// Before using this value, you may want to check the `HasMaxValue` property
    /// which specifies if a max value check is needed.
    /// </summary>
    /// <remarks>Check HasMaxValue before using this</remarks>
    public decimal MaxValue { get; set; } = decimal.MaxValue;

    /// <summary>
    /// If true, the value of MaxValue itself is allowed for this value.  So
    /// `value <= MaxValue`. If false, MaxValue itself is NOT allowed, so `value < MaxValue`
    /// </summary>
    public bool IsMaxValueInclusive { get; set; } = true;

    /// <summary>
    /// For string based values, this will be the minimum length of the string.
    /// Before using this value you may want to check if HasMinLength is true.
    /// </summary>
    /// <remarks>Check HasMinLength before using this</remarks>
    public int MinLength { get; set; } = 0;

    /// <summary>
    /// For string based values, this will be true if the value has a minimum length
    /// requirement.
    /// </summary>
    public bool HasMinLength { get; set; } = false;

    /// <summary>
    /// For string based values, this will be the max length of the string.
    /// Before using this value you may want to check if HasMaxLength is true.
    /// </summary>
    /// <remarks>Check HasMaxLength before using this</remarks>
    public int MaxLength { get; set; } = int.MaxValue;

    /// <summary>
    /// For string based values, this will be true if the value has a maximum length
    /// requirement.
    /// </summary>
    public bool HasMaxLength { get; set; } = false;

    /// <summary>
    /// A RegEx pattern following ECMA-262 syntax for this field. This can be used
    /// to evaluate input on the client before sending to the server. 
    /// </summary>
    /// <remarks>This will be an empty string if not specified.</remarks>
    public string Pattern { get; set; } = string.Empty;

    /// <summary>
    /// If true, this value can be null, even if the underlying type is a value type.
    /// For a string, this does not include empty strings.
    /// This is slightly different from IsRequired = false in the sense that
    /// the value can be present on the request and explicitly set to null.
    /// </summary>
    public bool IsNullable { get; set; } = false;
}