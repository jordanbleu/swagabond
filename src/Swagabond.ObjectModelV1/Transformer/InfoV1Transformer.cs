using Microsoft.OpenApi.Models;

namespace Swagabond.ObjectModelV1.Transformer;

public interface IInfoV1Transformer
{
    InfoV1 FromOpenApi(OpenApiInfo? info);
}

public class InfoV1Transformer : IInfoV1Transformer
{
    public InfoV1 FromOpenApi(OpenApiInfo? info)
    {
        var apiInfo = new InfoV1();

        apiInfo.ContactName = info?.Contact?.Name ?? string.Empty;
        apiInfo.ContactUrl = info?.Contact?.Url?.ToString() ?? string.Empty;
        apiInfo.ContactEmail = info?.Contact?.Email ?? string.Empty;
        
        apiInfo.TermsOfServiceUrl = info?.TermsOfService?.ToString() ?? string.Empty;
        
        apiInfo.LicenseUrl = info?.License?.Url?.ToString() ?? string.Empty;
        apiInfo.LicenseName = info?.License?.Name ?? string.Empty;
        
        return apiInfo;
    }
}