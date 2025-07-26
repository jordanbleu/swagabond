using System.Collections.ObjectModel;
using Swagabond.ObjectModelV1.Interfaces;

namespace Swagabond.ObjectModelV1;

/// <summary>
/// The Root object for the entire API.
/// </summary>
public class ApiV1 : IObjectV1, INamedObject
{

    /// <summary>
    /// The name of the API, formatted a PascalCase string with no spaces or special characters.
    /// </summary>
    public string Name { get; internal set; } = string.Empty;
    
    /// <summary>
    /// The original, raw name of the API.  May contain special characters or spaces, so not great for
    /// generating code or filenames.
    /// </summary>
    public string Title { get; internal set; } = string.Empty;
    
    /// <summary>
    ///  This should never be true.  Feel free to ignore :)
    /// </summary>
    public bool IsEmpty { get; internal set; } = true;
    
    /// <summary>
    /// A brief description of the API
    /// </summary>
    public string Description { get; internal set; } = string.Empty;
    
    /// <summary>
    /// A string representing the version of your API
    /// </summary>
    public string Version { get; internal set; } = string.Empty;
    
    /// <summary>
    /// The version of the current API spec that this object model is based on.  This will vary based on
    /// what type of API spec is being mapped.
    /// </summary>
    public string SpecVersion { get; internal set; } = string.Empty;
    
    /// <summary>
    /// The type of API spec the model was mapped from.  
    /// </summary>
    public SpecTypeV1 SpecType { get; internal set; } = SpecTypeV1.OpenApi;
    
    /// <summary>
    /// Various extra information about the API such as ToS, Contact Info, etc
    /// </summary>
    public InfoV1 Info { get; internal set; } = InfoV1.Empty;
    
    /// <summary>
    /// Generally, an outside link to external documentation
    /// </summary>
    public HrefV1 ExternalDocumentationLink { get; internal set; } = HrefV1.Empty;
    
    /// <summary>
    /// List of each path (aka route) exposed by the API.  Each path item also contains a
    /// list of operations that can be performed on that route.
    /// </summary>
    public List<PathV1> Paths { get; internal set; } = new();
    
    /// <summary>
    /// All the schema definitions referenced by the entire API.  A schema definition defines the properties of a
    /// complex object. 
    /// </summary>
    public List<SchemaDefinitionV1> Schemas { get; internal set; } = new();

    /// <summary>
    /// List of extensions on the API. Extensions can contain any arbitrary data.
    /// </summary>
    public List<ExtensionV1> Extensions { get; internal set; } = new();

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
    /// This is populated via Swagabond itself, generally via Template Instructions. The data here isn't necessarily
    /// related to the spec itself.  
    /// </summary>
    public Dictionary<string, string> Metadata { get; internal set; } = new();
    
    
    /// <summary>
    /// A flattened list of all operations that are defined in the API, by any path.
    /// </summary>
    public IEnumerable<OperationV1> Operations => Paths.SelectMany(p => p.Operations);

    /// <summary>
    /// List of base or direct URLs that host the API.
    /// </summary>
    public List<ServerV1> Servers { get; set; } = new();

    /// <summary>
    /// List of BaseUrls registered for this server.  Based on the 'servers'
    /// that are defined by your API spec.
    /// </summary>
    public IEnumerable<string> BaseUrls => Servers.Select(s => s.Url);

    public override string ToString()
    {
        return "ApiV1 " + Name;
    }

    public static readonly ApiV1 Empty = new();
}