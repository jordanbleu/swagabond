using Castle.Core.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Moq;
using Swagabond.Core;
using Swagabond.Core.Exceptions;
using Swagabond.Core.Mappers;

namespace Swagabond.Core.Tests;

public class MapperTests
{
    [Fact]
    public void MapFromStream_ThrowsExceptionForInvalidSpec_WithFailOnDefinitionWarning()
    {
        // var logger = new Mock<ILogger>();
        // var sut = new OpenApiMapper(logger.Object);
        //
        // var request = new MapperRequest { FailOnDefinitionWarning = true };
        //
        // var stream = new MemoryStream();
        // var result = new ReadResult
        // {
        //     OpenApiDocument = new OpenApiDocument(),
        //     OpenApiDiagnostic = new OpenApiDiagnostic()
        // };
        //
        // result.OpenApiDiagnostic.Warnings.Add(new("test", "test"));
        //
        // // Act
        // Func<Task> act = async () => await sut.MapFromStream(request, stream);
        //
        // // Assert
        // act.Should().Throw<InvalidApiSpecException>();
    }
}