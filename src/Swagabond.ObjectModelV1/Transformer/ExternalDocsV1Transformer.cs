using Microsoft.OpenApi.Models;

namespace Swagabond.ObjectModelV1.Transformer;

public interface IExternalDocsV1Transformer
{
    HrefV1 FromOpenApi(OpenApiExternalDocs? openApiExternalDocs);
}

public class ExternalDocsV1Transformer : IExternalDocsV1Transformer
{
    public HrefV1 FromOpenApi(OpenApiExternalDocs? openApiExternalDocs)
    {
        if (openApiExternalDocs == null)
            return HrefV1.Empty;
        
        return new HrefV1()
        {
            IsEmpty = false,
            Text = openApiExternalDocs.Description ?? string.Empty,
            Url = openApiExternalDocs.Url?.ToString() ?? string.Empty
        };
        
    }

}