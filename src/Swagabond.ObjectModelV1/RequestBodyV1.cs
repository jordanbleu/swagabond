using Swagabond.ObjectModelV1.Interfaces;

namespace Swagabond.ObjectModelV1;

/// <summary>
/// Contains information about a request body for an operation in an API.
/// A request body is a payload sent to an API endpoint, typically as Json or XML.
/// </summary>
public class RequestBodyV1 : IObjectV1, INamedObject
{
    /// <summary>
    /// If this is true, there is no response body.
    /// </summary>
    public bool IsEmpty { get; internal set; } = true;
    /// <summary>
    /// A cleanly formatted name for this request body in PascalCase. Suitable for code or filenames.
    /// Something to note, if your API spec has explicit schema definitions for each request (which is
    /// common), you can actually use that instead if you prefer.  Simply grab the info you need from the
    /// .Schema property instead. 
    /// </summary>
    /// <example>RestaurantsGetRequest (for a GET operation on '/restaurants')</example>
    public string Name { get; internal set; } = string.Empty;
    /// <summary>
    /// a more beautiful and gorgeous title for the request body. Not suitable for filenames, only
    /// for display purposes.
    /// </summary>
    /// <example>GET /restaurants RequestBody (for a GET operation on '/restaurants')</example>
    public string Title { get; internal set; } = string.Empty;
    /// <summary>
    /// A description of the request body.
    /// </summary>
    public string Description { get; internal set; } = string.Empty;

    /// <summary>
    /// The schema definition of the request body
    /// </summary>
    public SchemaDefinitionV1 Schema { get; set; } = new();
    
    /// <summary>
    /// The API that this request body belongs to.
    /// </summary>
    public ApiV1 Api { get; internal set; } = ApiV1.Empty;
    
    /// <summary>
    /// The operation that this request body belongs to
    /// </summary>
    public OperationV1 Operation { get; internal set; } = OperationV1.Empty;
    
    public static readonly RequestBodyV1 Empty = new();
    
}