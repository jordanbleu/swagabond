using Swagabond.ObjectModelV1.Interfaces;

namespace Swagabond.ObjectModelV1;

/// <summary>
/// Defines a response shape for a particular HTTP status code in an API operation.
/// </summary>
public class ResponseBodyV1 : IObjectV1, INamedObject
{
    /// <summary>
    /// If true, there is no response body.
    /// </summary>
    public bool IsEmpty { get; set; } = true;
    /// <summary>
    /// A cleanly formatted name for this response body in PascalCase. Suitable for code or filenames.
    /// Something to note, if your API spec has explicit schema definitions for each response (which is
    /// common), you can actually use that instead if you prefer.  Simply grab the info you need from the
    /// .Schema property instead. 
    /// </summary>
    /// <example>Restaurants200GetResponse (for a GET operation on '/restaurants' returning http 200)</example>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// A more beautiful and gorgeous title for the response body. Not suitable for filenames, only for
    /// display purposes.
    /// </summary>
    public string Title { get; set; } = string.Empty;
    /// <summary>
    /// A brief description of the response body.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The status code that this response shape is for  
    /// </summary>
    public int StatusCode { get; set; }

    
    /// <summary>
    /// A unique identifier for this individual response shape.  Used to differentiate each
    /// response body.
    /// </summary>
    public string ResponseId { get; set; } = string.Empty;

    /// <summary>
    /// The schema definition of the response body.
    /// </summary>
    public SchemaDefinitionV1 Schema { get; set; } = new();
    
    /// <summary>
    /// The API that this response body belongs to. 
    /// </summary>
    public ApiV1 Api { get; set; } = ApiV1.Empty;
    /// <summary>
    /// The operation that this response body belongs to.
    /// </summary>
    public OperationV1 Operation {  get; set; } = OperationV1.Empty;
    
    public static readonly ResponseBodyV1 Empty = new();
}