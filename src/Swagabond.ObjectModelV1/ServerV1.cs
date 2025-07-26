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
    
    private Dictionary<string, string>? _extensionDictionary = null;

    /// <summary>
    /// A dictionary of extensions where the key is the extension name and the value
    /// is its value.  This allows you to bind directly to known keys instead of iterating
    /// over the list of extensions. Values can be accessed via `ExtensionDictionary["x-myKey"]`
    /// </summary>
    public Dictionary<string, string> ExtensionDictionary {
        get
        {
            if (_extensionDictionary == null)
            {
                _extensionDictionary = Extensions.ToDictionary(e => e.Name, e => e.Value);
            }

            return _extensionDictionary;
        }
    }

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