using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Exceptions;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Moq;
using Moq.AutoMock;
using Swagabond.Core;
using Swagabond.Core.Exceptions;
using Swagabond.Core.Mappers;
using Shouldly;
using Swagabond.Core.Parsers;

namespace Swagabond.Tests.Core;

public class MapperRequestTests
{
    [Fact]
    public void FailedTes()
    {
        Assert.Fail("asdf");
    }

    [Fact]
    public async Task MapFromStream_DoesNotThrowExceptionForInvalidSpec_WhenConfiguredNotTo()
    {
        var autoMocker = new AutoMocker();
        
        var logger = autoMocker.GetMock<ILogger<OpenApiMapper>>();
        
        logger.Setup(x => x.Log(
            LogLevel.Warning,
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            It.IsAny<OpenApiException>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()
        ));
        
        var openApiDocument = new OpenApiDocument();
        
        var parser = autoMocker.GetMock<IMicrosoftSwaggerParser>();
            
        parser.Setup(x=>x.ParseAsOpenApiDocument(It.Is<Stream>(s=>s!=null)))
            .ReturnsAsync(new ReadResult()
            {
                OpenApiDocument = openApiDocument,
                OpenApiDiagnostic = new OpenApiDiagnostic()
                {
                    Warnings = new List<OpenApiError>()
                    {
                        new(new OpenApiException())
                        {
                            Message = "Warning"
                        }
                    },
                    Errors = new List<OpenApiError>()
                    {
                        new(new OpenApiException())
                        {
                            Message = "Error"
                        }
                    }
                }});
        
        var mapper = new OpenApiMapper(logger.Object, parser.Object);

        var request = new MapperRequest()
        {
            FailOnDefinitionError = false,
            FailOnDefinitionWarning = false
        };
        var stream = new MemoryStream();
        var act = () => mapper.MapFromStream(request, stream);

        // Assert
        await act.ShouldNotThrowAsync();
        
    }
    
    
    [Fact]
    public async Task MapFromStream_ThrowsExceptionForInvalidSpec_WhenFailOnDefinitionWarningIsTrue()
    {
        var autoMocker = new AutoMocker();
        
        var logger = autoMocker.GetMock<ILogger<OpenApiMapper>>();
        
        logger.Setup(x => x.Log(
            LogLevel.Warning,
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            It.IsAny<OpenApiException>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()
        ));
        
        
        var openApiDocument = new OpenApiDocument();
        
        var parser = autoMocker.GetMock<IMicrosoftSwaggerParser>();
            
        parser.Setup(x=>x.ParseAsOpenApiDocument(It.Is<Stream>(s=>s!=null)))
            .ReturnsAsync(new ReadResult()
            {
                OpenApiDocument = openApiDocument,
                OpenApiDiagnostic = new OpenApiDiagnostic()
                {
                    Warnings = new List<OpenApiError>()
                    {
                        new(new OpenApiException())
                        {
                            Message = "Warning"
                        }
                    }
                }});
        
        var mapper = new OpenApiMapper(logger.Object, parser.Object);

        var request = new MapperRequest()
        {
            FailOnDefinitionWarning = true
        };

        var stream = new MemoryStream();
        var act = () => mapper.MapFromStream(request, stream);

        // Assert
        (await act.ShouldThrowAsync<InvalidApiSpecException>()).Message.ShouldContain("warning", Case.Insensitive);
    }
    
    [Fact]
    public async Task MapFromStream_ThrowsExceptionForInvalidSpec_WhenFailOnDefinitionErrorIsTrue()
    {
        var autoMocker = new AutoMocker();
        
        var logger = autoMocker.GetMock<ILogger<OpenApiMapper>>();
        
        logger.Setup(x => x.Log(
            LogLevel.Warning,
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            It.IsAny<OpenApiException>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()
        ));
        
        
        var openApiDocument = new OpenApiDocument();
        
        var parser = autoMocker.GetMock<IMicrosoftSwaggerParser>();
            
        parser.Setup(x=>x.ParseAsOpenApiDocument(It.Is<Stream>(s=>s!=null)))
            .ReturnsAsync(new ReadResult()
            {
                OpenApiDocument = openApiDocument,
                OpenApiDiagnostic = new OpenApiDiagnostic()
                {
                    Errors = new List<OpenApiError>()
                    {
                        new(new OpenApiException())
                        {
                            Message = "Error"
                        }
                    }
                }});
        
        var mapper = new OpenApiMapper(logger.Object, parser.Object);

        var request = new MapperRequest()
        {
            FailOnDefinitionWarning = true
        };

        var stream = new MemoryStream();
        var act = () => mapper.MapFromStream(request, stream);

        // Assert
        (await act.ShouldThrowAsync<InvalidApiSpecException>()).Message.ShouldContain("error", Case.Insensitive);
    }
}