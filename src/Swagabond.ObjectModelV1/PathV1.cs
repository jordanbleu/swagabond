using Swagabond.ObjectModelV1.Interfaces;

namespace Swagabond.ObjectModelV1;

/// <summary>
/// A path is simply a route which is based on the API's base URL. A path contains a list of
/// operations (such as GET, POST, PUT, etc.) that can be performed against that path.
/// </summary>
public class PathV1 : IObjectV1, INamedObject
{
    /// <summary>
    /// If true, this path is empty.
    /// </summary>
    public bool IsEmpty { get; internal set; } = true;
    /// <summary>
    /// A cleanly formatted name for this path in PascalCase. 
    /// </summary>
    /// <example>RestaurantsMenuFood (for the path '/restaurants/menu/food')</example>
    public string Name { get; internal set; }= string.Empty;
    /// <summary>
    /// The unfiltered path, not suitable for code or filenames.  This is actually the
    /// same thing as the 'route' property because I don't really know a prettier way to
    /// name a path. Despite that fun fact, this should NOT be used for anything other than
    /// display purposes.
    /// </summary>
    /// <example>/restaurants/menu/food</example>
    public string Title { get; internal set; }= string.Empty;
    /// <summary>
    /// A brief description of the path
    /// </summary>
    public string Description { get; internal set; } = string.Empty;

    /// <summary>
    /// This is the actual route of the path.  This is safe to use for actual routing logic in generated
    /// code.
    /// </summary>
    /// <example>/restaurants/menu/food</example>
    public string Route { get; internal set; } = string.Empty;

    /// <summary>
    /// List of all endpoints (and their details) for this path
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