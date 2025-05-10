using Microsoft.OpenApi.Interfaces;

namespace Swagabond.ObjectModelV1.Transformer;

public interface IExtensionV1Transformer
{
    List<ExtensionV1> TransformExtensions(IDictionary<string, IOpenApiExtension> extensions);
}

public class ExtensionV1Transformer : IExtensionV1Transformer
{
    public List<ExtensionV1> TransformExtensions(IDictionary<string, IOpenApiExtension> extensions)
    {
        var extensionList = new List<ExtensionV1>();

        foreach (var kvp in extensions)
        {
            var extension = new ExtensionV1
            {
                Name = kvp.Key,
                Value = kvp.Value.ToString() ?? string.Empty
            };

            extensionList.Add(extension);
        }

        return extensionList;
    }
}