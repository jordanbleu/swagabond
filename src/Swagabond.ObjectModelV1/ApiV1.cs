using Swagabond.ObjectModelV1.Interfaces;

namespace Swagabond.ObjectModelV1;

/// <summary>
/// The Root object for the entire API.
///
/// <remarks>ObjectModel V1</remarks>
/// </summary>
public class ApiV1 : IObjectV1, INamedObject
{
    public bool IsEmpty { get; internal set; } = true;

    public string Name { get; internal set; } = string.Empty;
    
    public string Title { get; internal set; } = string.Empty;
    
    public string Description { get; internal set; } = string.Empty;
    
    /// <summary>
    /// The version of your API
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
    /// Link to external documentation
    /// </summary>
    public HrefV1 ExternalDocumentationLink { get; internal set; } = HrefV1.Empty;
    
    /// <summary>
    /// List of each path exposed by the API.
    /// </summary>
    public List<PathV1> Paths { get; internal set; } = new();
    
    /// <summary>
    /// List of every 'schema definition' used in the entire API.  A schema definition can be thought of
    /// more or less like a 'class', or a named group of properties.  
    /// </summary>
    public List<SchemaDefinitionV1> Schemas { get; internal set; } = new();

    /// <summary>
    /// Contains additional properties which are populated by Swagabond itself rather than the api spec.
    /// </summary>
    public Dictionary<string, string> Metadata { get; internal set; } = new();
    
    
    /// <summary>
    /// Selects all operations from all paths on the api
    /// </summary>
    public IEnumerable<OperationV1> Operations => Paths.SelectMany(p => p.Operations);

    public override string ToString()
    {
        return "ApiV1 " + Name;
    }

    public static readonly ApiV1 Empty = new();
}