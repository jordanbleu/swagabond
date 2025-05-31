using Swagabond.ObjectModelV1.Interfaces;

namespace Swagabond.ObjectModelV1;

/// <summary>
/// An operation is a type of request that can be made against a particular path.
/// An example would be "GET /restaurants/{restaurantId}/menu", which is a GET operation against the
/// '/restaurants/{restaurantId}/menu' path.  Another operation would be "POST /restaurants/{restaurantId}/menu",
/// which is a POST operation against the same path.
/// </summary>
public class OperationV1 : IObjectV1, INamedObject
{
    /// <summary>
    /// If true, this is an empty operation.
    /// </summary>
    public bool IsEmpty { get; internal set; } = true;
    
    /// <summary>
    /// The name of the operation, formatted as PascalCase in a way that is suitable for use in code or filenames.
    /// The name will always be unique for each operation.
    /// </summary>
    /// <example>GetRestaurantData</example>
    public string Name { get; internal set; } = string.Empty;
    
    /// <summary>
    /// A more beautiful and gorgeous title for the operation.  Not suitable for filenames.
    /// </summary>
    /// <example>GET /Restaurant/Data</example>
    public string Title { get; internal set; } = string.Empty;
    /// <summary>
    /// A brief description of this operation.
    /// </summary>
    public string Description { get; internal set; } = string.Empty;

    /// <summary>
    /// The Http Method for the operation
    /// </summary>
    public string Method { get; set; } = string.Empty;
    
    /// <summary>
    /// A list of query parameters for this operation.  Query parameters
    /// are placed in the URL after the '?' character.
    /// </summary>
    public List<SchemaReferenceV1> QueryParameters { get; set; } = new();
    
    /// <summary>
    /// List of header parameters for this operation.  Header parameters
    /// go in http headers.
    /// </summary>
    public List<SchemaReferenceV1> HeaderParameters { get; set; } = new();

    /// <summary>
    /// List of path parameters for this operation.  Path parameters are integrated
    /// into the url path, and are typically used to identify a specific resource.
    /// For example, in the url '/restaurants/{restaurantId}/menu', the {restaurantId} is a path parameter.
    /// </summary>
    public List<SchemaReferenceV1> PathParameters { get; set; } = new();
    
    /// <summary>
    /// List of cookie parameters that can (or should) be sent with the request.
    /// </summary>
    public List<SchemaReferenceV1> CookieParameters { get; set; } = new();
    
    /// <summary>
    /// Information about the request Body (payload) for the operation, typically in Json or XML format.
    /// </summary>
    public RequestBodyV1 RequestBody { get; set; } = RequestBodyV1.Empty;

    /// <summary>
    /// List of each documented response type for this operation, and the expected
    /// response body shape that goes along with it.
    /// </summary>
    public List<ResponseBodyV1> ResponseBodies { get; set; } = new();

    /// <summary>
    /// A fallback response body for status codes that don't have a dedicated response body.
    /// This will sometimes be the ONLY response body if only one is defined.  Otherwise, more
    /// specific response bodies should be considered, and this one should be the fallback.
    /// </summary>
    public ResponseBodyV1 DefaultResponseBody { get; set; } = ResponseBodyV1.Empty;

    /// <summary>
    /// Assuming response body will be the same for any success http status (200 - 299) this will
    /// be the first success response body or the default fallback.
    ///
    /// If your api returns different response shapes for different success status codes, you shouldn't
    /// use this, and instead you'll need more nuanced logic.
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
    /// Assuming response body will be the same for any error http status (300+) this will
    /// be the first error response body or the default fallback.
    ///
    /// If your api returns different response shapes for different error status codes, you shouldn't
    /// use this, and instead you'll need more nuanced logic.
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
    
    /// <summary>
    /// The Path this operation belongs to. 
    /// </summary>
    public PathV1 Path { get; set; } = PathV1.Empty;
    /// <summary>
    /// The API this operation belongs to.
    /// </summary>
    public ApiV1 Api { get; set; } = ApiV1.Empty;

    /// <summary>
    /// List of arbitrary extensions for this operation
    /// </summary>
    public List<ExtensionV1> Extensions { get; set; } = new();
    public override string ToString()
    {
        return $"OperationV1 {Method} {Path.Route}";
    }

    public static OperationV1 Empty = new();

}