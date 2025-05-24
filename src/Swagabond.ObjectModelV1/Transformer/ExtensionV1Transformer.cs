using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Swagabond.ObjectModelV1.Extensions;

namespace Swagabond.ObjectModelV1.Transformer;

public interface IExtensionV1Transformer
{
    List<ExtensionV1> FromOpenApi(IDictionary<string, IOpenApiExtension> extensions);
}

public class ExtensionV1Transformer : IExtensionV1Transformer
{
    public List<ExtensionV1> FromOpenApi(IDictionary<string, IOpenApiExtension> extensions)
    {
        var extensionList = new List<ExtensionV1>();

        foreach (var kvp in extensions)
        {
            var extension = new ExtensionV1
            {
                Name = kvp.Key,
                Value = ((IOpenApiAny)kvp.Value).WriteAsString()
            };

            extensionList.Add(extension);
        }

        return extensionList;
    }
}