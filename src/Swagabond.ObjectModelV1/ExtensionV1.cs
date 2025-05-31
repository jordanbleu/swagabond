namespace Swagabond.ObjectModelV1;

/// <summary>
/// An single arbitrary extension value, can contain anything.
/// </summary>
public class ExtensionV1
{
    /// <summary>
    /// The extension name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// The extension value
    /// </summary>
    public string Value { get; set; } = string.Empty;
}