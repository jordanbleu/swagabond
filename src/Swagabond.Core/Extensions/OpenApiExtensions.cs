using System.Globalization;
using Microsoft.OpenApi.Any;

namespace Swagabond.Core.Extensions;

public static class OpenApiExtensions
{
    /// <summary>
    /// Writes out the OpenApiAny as a stringified representation of the value
    /// </summary>
    /// <param name="openApiAny"></param>
    /// <returns></returns>
    public static string WriteAsString(this IOpenApiAny openApiAny) => openApiAny switch
    {
        OpenApiString s => s.Value,
        OpenApiInteger i => i.Value.ToString(),
        OpenApiLong l => l.Value.ToString(),
        OpenApiFloat f => f.Value.ToString(CultureInfo.InvariantCulture),
        OpenApiDouble d => d.Value.ToString(CultureInfo.InvariantCulture),
        OpenApiBoolean b => b.Value.ToString(),
        OpenApiArray a => string.Join(",", a.Select(x => x.WriteAsString())),
        OpenApiObject o => string.Join(",", o.Select(x => $"{x.Key}:{x.Value.WriteAsString()}")),
        _ => string.Empty
    };
    

}