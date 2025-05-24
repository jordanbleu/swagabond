using Swagabond.ObjectModelV1.Interfaces;

namespace Swagabond.ObjectModelV1;

public class PathV1 : IObjectV1, INamedObject
{
    public bool IsEmpty { get; internal set; } = true;
    public string Name { get; internal set; }= string.Empty;
    public string Title { get; internal set; }= string.Empty;
    public string Description { get; internal set; } = string.Empty;

    /// <summary>
    /// the actual route to the endpoints, based on the API's root url
    /// </summary>
    public string Route { get; internal set; } = string.Empty;

    /// <summary>
    /// List of all endpoints (and their details) under this path
    /// </summary>
    public List<OperationV1> Operations { get; set; } = new();

    /// <summary>
    /// The API this path belongs to
    /// </summary>
    public ApiV1 Api { get; set; } = ApiV1.Empty;

    /// <summary>
    /// List of arbitrary extensions for this path
    /// </summary>
    public List<ExtensionV1> Extensions { get; set; } = new();

    public override string ToString()
    {
        return $"PathV1 {Route}";
    }

    public static readonly PathV1 Empty = new();
    
}