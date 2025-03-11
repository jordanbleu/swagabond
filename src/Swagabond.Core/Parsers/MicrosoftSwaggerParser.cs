using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;

namespace Swagabond.Core.Parsers;

public interface IMicrosoftSwaggerParser
{
    Task<ReadResult> ParseAsOpenApiDocument(Stream swaggerStream);
}

/// <summary>
/// Utilizes Microsoft.OpenApi library for parsing
/// </summary>
public class MicrosoftSwaggerParser : IMicrosoftSwaggerParser
{
    private readonly OpenApiStreamReader _openApiReader;

    public MicrosoftSwaggerParser(OpenApiStreamReader openApiReader)
    {
        _openApiReader = openApiReader;
    }

    public Task<ReadResult> ParseAsOpenApiDocument(Stream swaggerStream)
    {
        return _openApiReader.ReadAsync(swaggerStream);
    }
}