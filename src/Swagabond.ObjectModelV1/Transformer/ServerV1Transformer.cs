using Microsoft.OpenApi.Models;

namespace Swagabond.ObjectModelV1.Transformer;

public interface IServerV1Transformer
{
    ServerV1 FromOpenApi(OpenApiServer openApiServer);
}

public class ServerV1Transformer : IServerV1Transformer
{
    private readonly IExtensionV1Transformer _extensionV1Transformer;

    public ServerV1Transformer(IExtensionV1Transformer extensionV1Transformer)
    {
        _extensionV1Transformer = extensionV1Transformer;
    }


    public ServerV1 FromOpenApi(OpenApiServer openApiServer)
    {
        var server = new ServerV1
        {
            Extensions = _extensionV1Transformer.FromOpenApi(openApiServer.Extensions),
            Url = openApiServer.Url,
            Description = openApiServer.Description
        };

        return server;
    }

}