namespace Swagabond.Core;

public class MapperRequest
{
    /// <summary>
    /// If true, the process will throw an exception if any errors are found in the spec itself.
    /// Example, Duplicate keys, bad references, bad syntax, etc.
    /// </summary>
    public bool FailOnDefinitionError { get; init; } = true;
    
    /// <summary>
    /// If true, the process will throw an exception if any warnings are found in the spec itself.
    /// Example, Unused components, etc
    /// </summary>
    public bool FailOnDefinitionWarning { get; init; } = false;

    /// <summary>
    /// Any arbitrary metadata to be passed along, and made available to the templates.
    /// </summary>
    public Dictionary<string, string> Metadata { get; init; } = new();
}