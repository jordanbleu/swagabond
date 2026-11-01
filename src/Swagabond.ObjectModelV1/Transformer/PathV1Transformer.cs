using Microsoft.OpenApi.Models;
using Swagabond.ObjectModelV1.Extensions;

namespace Swagabond.ObjectModelV1.Transformer;

public interface IPathV1Transformer
{
    PathV1 FromOpenApi(KeyValuePair<string, OpenApiPathItem> path, ApiV1 apiV1);
}

public class PathV1Transformer : IPathV1Transformer
{
    private readonly IOperationV1Transformer _operationV1Transformer;
    private readonly IExtensionV1Transformer _extensionV1Transformer;

    public PathV1Transformer(IOperationV1Transformer operationV1Transformer, IExtensionV1Transformer extensionV1Transformer)
    {
        _operationV1Transformer = operationV1Transformer;
        _extensionV1Transformer = extensionV1Transformer;
    }

    public PathV1 FromOpenApi(KeyValuePair<string, OpenApiPathItem> path, ApiV1 apiV1)
    {
        var route = path.Key;
        var p = path.Value;

        var apiPath = new PathV1
        {
            IsEmpty = false,
            Name = route.ToClassName(),
            Title = route,
            Description = p.Description ?? route,
            Route = route,
            Extensions = _extensionV1Transformer.FromOpenApi(p.Extensions)
        };


        foreach (var op in p.Operations)
        {
            apiPath.Operations.Add(_operationV1Transformer.FromOpenApi(op, apiPath, apiV1));
        }

        apiPath.Api = apiV1;

        return apiPath;
    }

}