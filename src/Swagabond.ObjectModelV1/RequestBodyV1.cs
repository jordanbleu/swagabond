using Swagabond.ObjectModelV1.Interfaces;

namespace Swagabond.ObjectModelV1;

public class RequestBodyV1 : IObjectV1, INamedObject
{
    public bool IsEmpty { get; internal set; } = true;
    public string Name { get; internal set; } = string.Empty;
    public string Title { get; internal set; } = string.Empty;
    public string Description { get; internal set; } = string.Empty;

    public SchemaDefinitionV1 Schema { get; set; } = new();
    
    public ApiV1 Api { get; internal set; } = ApiV1.Empty;
    
    public static readonly RequestBodyV1 Empty = new();
    
}