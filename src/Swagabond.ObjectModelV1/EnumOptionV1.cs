namespace Swagabond.ObjectModelV1;

/// <summary>
/// The name and underlying value for the enum option 
/// </summary>
public class EnumOptionV1
{
    /// <summary>
    /// The enum name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// The underlying enum value
    /// </summary>
    public string Value { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"EnumOptionV1 {Name} = {Value}";
    }
}