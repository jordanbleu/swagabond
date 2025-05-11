using Swagabond.ObjectModelV1.Interfaces;

namespace Swagabond.ObjectModelV1;

public class ResponseBodyV1 : IObjectV1, INamedObject
{
    public bool IsEmpty { get; set; } = true;
    public string Name { get; set; } = string.Empty;
    
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The status code that this response shape is for  
    /// </summary>
    public int StatusCode { get; set; }

    
    /// <summary>
    /// Identifier for this individual response shape.  Used to differentiate each
    /// response body.
    /// </summary>
    public string ResponseId { get; set; } = string.Empty;

    public SchemaDefinitionV1 Schema { get; set; } = new();
    
    public ApiV1 Api { get; set; } = ApiV1.Empty;
    public OperationV1 Operation {  get; set; } = OperationV1.Empty;
    
    public static readonly ResponseBodyV1 Empty = new();
}