namespace Swagabond.ObjectModelV1;

/// <summary>
/// Represents a 'server' that the API is hosted at.
/// </summary>
public class ServerV1
{
    /// <summary>
    /// A list of arbitrary extensions for the server
    /// </summary>
    public List<ExtensionV1> Extensions { get; set; }

    /// <summary>
    /// A description for the server
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// The server's base URL.  This can either be a relative or direct URL, based
    /// on the original API spec.  The URL can also contain variables, but Swagabond
    /// does not currently support variable definitions for server URLs.
    /// </summary>
    public string Url { get; set; }
}