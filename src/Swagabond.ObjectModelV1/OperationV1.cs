using Swagabond.ObjectModelV1.Interfaces;

namespace Swagabond.ObjectModelV1;

public class OperationV1 : IObjectV1, INamedObject
{
    public bool IsEmpty { get; internal set; } = true;
    public string Name { get; internal set; } = string.Empty;
    public string Title { get; internal set; } = string.Empty;
    public string Description { get; internal set; } = string.Empty;

    /// <summary>
    /// The Http Method for the operation
    /// </summary>
    public string Method { get; set; } = string.Empty;
    
    public List<SchemaReferenceV1> QueryParameters { get; set; } = new();
    
    public List<SchemaReferenceV1> HeaderParameters { get; set; } = new();

    public List<SchemaReferenceV1> PathParameters { get; set; } = new();
    
    public List<SchemaReferenceV1> CookieParameters { get; set; } = new();
    
    /// <summary>
    /// Request Body / payload for the operation
    /// </summary>
    public RequestBodyV1 RequestBody { get; set; } = RequestBodyV1.Empty;

    /// <summary>
    /// The Response Body
    /// </summary>
    public List<ResponseBodyV1> ResponseBodies { get; set; } = new();

    /// <summary>
    /// A fallback response body for status codes that don't have a dedicated response body.
    /// </summary>
    public ResponseBodyV1 DefaultResponseBody { get; set; } = ResponseBodyV1.Empty;

    /// <summary>
    /// If the response body will be the same for any success http status (200 - 299) this will
    /// be the first success response body or the default fallback.
    /// </summary>
    public ResponseBodyV1 SuccessResponseBody 
    {
        get 
        {
            var firstSuccessResponse = ResponseBodies.FirstOrDefault(r => !r.IsEmpty && r.StatusCode is > 199 and < 300);

            if (firstSuccessResponse != null)
                return firstSuccessResponse;

            return DefaultResponseBody;
        }
    }
    
    /// <summary>
    /// If the response body will be the same for any error http status ( >299 ) this will
    /// be the first error response body or the default fallback.
    /// </summary>
    public ResponseBodyV1 ErrorResponseBody 
    {
        get 
        {
            var firstErrorResponse = ResponseBodies.FirstOrDefault(r=>r.IsEmpty == false && r.StatusCode > 299 );
            
            if (firstErrorResponse != null)
                return firstErrorResponse;
            
            return DefaultResponseBody;
        }
    }
    
    public PathV1 Path { get; set; } = PathV1.Empty;
    public ApiV1 Api { get; set; } = ApiV1.Empty;

    public override string ToString()
    {
        return $"OperationV1 {Method} {Path.Route}";
    }

    public static OperationV1 Empty = new();

}