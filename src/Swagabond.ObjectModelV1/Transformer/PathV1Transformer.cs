using Microsoft.OpenApi.Models;
using Swagabond.ObjectModelV1.Extensions;

namespace Swagabond.ObjectModelV1.Transformer;

public interface IPathV1Transformer
{
    PathV1 FromOpenApi(KeyValuePair<string, OpenApiPathItem> path, ApiV1 apiV1);
}

public class PathV1Transformer : IPathV1Transformer
{
    private IOperationV1Transformer _operationV1Transformer;

    public PathV1Transformer(IOperationV1Transformer operationV1Transformer)
    {
        _operationV1Transformer = operationV1Transformer;
    }

    public PathV1 FromOpenApi(KeyValuePair<string, OpenApiPathItem> path, ApiV1 apiV1)
    {
        var route = path.Key;
        var p = path.Value;
        
        var apiPath = new PathV1();

        apiPath.IsEmpty = false;
        apiPath.Name = route.ToClassName();
        apiPath.Title = route;
        apiPath.Description = p.Description ?? route;
        apiPath.Route = route;

        foreach (var op in p.Operations)
        {
            apiPath.Operations.Add(_operationV1Transformer.FromOpenApi(op, apiPath, apiV1));
        }

        apiPath.Api = apiV1;
        
        return apiPath;
    }

}