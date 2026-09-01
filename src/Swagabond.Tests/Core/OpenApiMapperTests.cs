using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Moq;
using Moq.AutoMock;
using Shouldly;
using Swagabond.Core;
using Swagabond.Core.Exceptions;
using Swagabond.Core.Mappers;
using Swagabond.Core.Parsers;
using Swagabond.ObjectModelV1;
using Swagabond.ObjectModelV1.Transformer;

namespace Swagabond.Tests.Core;

public class OpenApiMapperTests
{
    private readonly AutoMocker _mocker = new();
    private readonly OpenApiMapper _target;

    public OpenApiMapperTests()
    {
        _mocker.GetMock<IApiV1Transformer>()
            .Setup(x => x.FromOpenApi(It.IsAny<TransformerV1Request>(), It.IsAny<OpenApiDocument>(), It.IsAny<string>()))
            .Returns(new ApiV1());

        _target = _mocker.CreateInstance<OpenApiMapper>();
    }

    [Fact]
    public async Task MapFromStreamV1_NoWarningsOrErrors_ReturnsApiV1()
    {
        SetupParser(warnings: 0, errors: 0);

        var result = await _target.MapFromStreamV1(new MapperRequest(), new MemoryStream());

        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task MapFromStreamV1_FailOnDefinitionWarning_WithWarnings_ThrowsInvalidApiSpecException()
    {
        SetupParser(warnings: 1, errors: 0);
        var request = new MapperRequest { FailOnDefinitionWarning = true };

        await Should.ThrowAsync<InvalidApiSpecException>(
            () => _target.MapFromStreamV1(request, new MemoryStream()));
    }

    [Fact]
    public async Task MapFromStreamV1_FailOnDefinitionWarning_False_WithWarnings_DoesNotThrow()
    {
        SetupParser(warnings: 1, errors: 0);
        var request = new MapperRequest { FailOnDefinitionWarning = false };

        await Should.NotThrowAsync(() => _target.MapFromStreamV1(request, new MemoryStream()));
    }

    [Fact]
    public async Task MapFromStreamV1_FailOnDefinitionError_WithErrors_ThrowsInvalidApiSpecException()
    {
        SetupParser(warnings: 0, errors: 1);
        var request = new MapperRequest { FailOnDefinitionError = true };

        await Should.ThrowAsync<InvalidApiSpecException>(
            () => _target.MapFromStreamV1(request, new MemoryStream()));
    }

    [Fact]
    public async Task MapFromStreamV1_FailOnDefinitionError_False_WithErrors_DoesNotThrow()
    {
        SetupParser(warnings: 0, errors: 1);
        var request = new MapperRequest { FailOnDefinitionError = false };

        await Should.NotThrowAsync(() => _target.MapFromStreamV1(request, new MemoryStream()));
    }

    [Fact]
    public async Task MapFromStreamV1_Metadata_IsPassedToTransformer()
    {
        SetupParser(warnings: 0, errors: 0);
        var metadata = new Dictionary<string, string> { ["env"] = "prod" };
        var request = new MapperRequest { Metadata = metadata };

        await _target.MapFromStreamV1(request, new MemoryStream());

        _mocker.GetMock<IApiV1Transformer>()
            .Verify(x => x.FromOpenApi(
                It.Is<TransformerV1Request>(r => r.Metadata == metadata),
                It.IsAny<OpenApiDocument>(),
                It.IsAny<string>()), Times.Once);
    }

    private void SetupParser(int warnings, int errors)
    {
        var diag = new OpenApiDiagnostic();
        for (var i = 0; i < warnings; i++)
            diag.Warnings.Add(new OpenApiError("", $"warning {i}"));
        for (var i = 0; i < errors; i++)
            diag.Errors.Add(new OpenApiError("", $"error {i}"));

        var readResult = new ReadResult
        {
            OpenApiDocument = new OpenApiDocument(),
            OpenApiDiagnostic = diag
        };

        _mocker.GetMock<IMicrosoftSwaggerParser>()
            .Setup(x => x.ParseAsOpenApiDocument(It.IsAny<Stream>()))
            .ReturnsAsync(readResult);
    }
}
