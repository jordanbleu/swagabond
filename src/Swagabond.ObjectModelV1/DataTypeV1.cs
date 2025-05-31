namespace Swagabond.ObjectModelV1;

/// <summary>
/// Represents the underlying type of a schema property.
/// </summary>
public enum DataTypeV1
{
    /// <summary>
    /// Represents a string value, which is a sequence of characters.
    /// </summary>
    String = 0,
    /// <summary>
    /// Represents a complex inner object, which can contain multiple properties and values.
    /// </summary>
    Object = 1,
    /// <summary>
    /// Represents a 32-bit signed integer value.
    /// </summary>
    Int32 = 2,
    /// <summary>
    /// Represents a 64-bit signed integer value.
    /// </summary>
    Int64 = 3,
    /// <summary>
    /// Represents a globally unique identifier (GUID), which is a 128-bit value used to uniquely identify objects.
    /// </summary>
    Guid = 4,
    /// <summary>
    /// Represents a date and time value, typically in UTC format.
    /// </summary>
    DateTime = 5,
    /// <summary>
    /// Represents a boolean value, which can be either true or false.
    /// </summary>
    Boolean = 6,
    /// <summary>
    /// Represents a floating-point number, which is a number that can have a fractional part.
    /// </summary>
    Float = 7,
    /// <summary>
    /// Represents a double-precision floating-point number, which is a number that can have a fractional part with higher precision than a float.
    /// </summary>
    Double = 8,
    /// <summary>
    /// Represents a decimal number, which is a fixed-point number that can represent values with high precision, often used for financial calculations.
    /// </summary>
    Decimal = 9,
}