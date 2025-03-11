using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Readers;
using Swagabond.Core.Exceptions;
using Swagabond.Core.ObjectModel;

namespace Swagabond.Core.Mappers;

/// <summary>
/// Maps stuff to the Swagabond object model
/// </summary>
public class OpenApiMapper
{
    private readonly ILogger<OpenApiMapper> _logger;
    private readonly OpenApiStreamReader _openApiReader;
    
    public OpenApiMapper(ILogger<OpenApiMapper> logger, OpenApiStreamReader openApiStreamReader)
    {
        _logger = logger;
        _openApiReader = openApiStreamReader;
    }

    /// <summary>
    /// Retrieves the data from the passed in stream, and maps it to our internal object model.
    /// The stream is then disposed of.
    /// </summary>
    /// <param name="request">Details for your request / how to handle some mappings</param>
    /// <param name="swaggerStream">a stream that points to either a swagger.json or a swagger.yaml</param>
    /// <returns></returns>
    public async Task<Api> MapFromStream(MapperRequest request, Stream swaggerStream)
    {
        _logger.LogInformation("Parsing OpenAPI document from stream");
        
        // This is first mapped by .net's library
        var result = await _openApiReader.ReadAsync(swaggerStream);

        ValidateApiSpec(request, result);

        var openApiDocument = result.OpenApiDocument;
        
        // The .net library's object is then mapped to our object model which is easier to write templates for.
        var api = Api.FromOpenApi(openApiDocument, result.OpenApiDiagnostic, request);
 
        await swaggerStream.DisposeAsync();
        return api;
    }
    
    private void ValidateApiSpec(MapperRequest request, ReadResult result)
    {
        var diag = result.OpenApiDiagnostic;
        
        foreach (var warning in diag.Warnings)
        {
            _logger.LogWarning("OpenAPI spec warning: {0}", warning.Message);
        }
        
        if (request.FailOnDefinitionWarning && diag.Warnings.Any())
        {
            throw new InvalidApiSpecException("OpenAPI spec contains warnings.");
        }
        
        foreach (var error in diag.Errors)
        {
            _logger.LogError("OpenAPI spec error: {0}", error.Message);
        }
        
        if (request.FailOnDefinitionError && diag.Errors.Any())
        {
            throw new InvalidApiSpecException("OpenAPI spec contains errors");
        }
    }
}