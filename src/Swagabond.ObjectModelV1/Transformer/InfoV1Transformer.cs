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
        var apiInfo = new InfoV1
        {
            ContactName = info?.Contact?.Name ?? string.Empty,
            ContactUrl = info?.Contact?.Url?.ToString() ?? string.Empty,
            ContactEmail = info?.Contact?.Email ?? string.Empty,

            TermsOfServiceUrl = info?.TermsOfService?.ToString() ?? string.Empty,

            LicenseUrl = info?.License?.Url?.ToString() ?? string.Empty,
            LicenseName = info?.License?.Name ?? string.Empty
        };

        return apiInfo;
    }
}