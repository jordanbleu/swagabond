namespace Swagabond.ObjectModelV1.Interfaces;

/// <summary>
/// Objects that have their own dedicated scope for templates should implement this interface.
/// </summary>
public interface INamedObject
{
    /// <summary>
    /// Concise name  with only alphanumeric characters.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Unfiltered title, may contain spaces and other characters.
    /// </summary>
    public string Title { get; }
    

    /// <summary>
    /// A longer description, may contain newlines
    /// </summary>
    public string Description { get; }

}