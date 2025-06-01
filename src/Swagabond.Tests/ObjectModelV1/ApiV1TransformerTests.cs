using AutoFixture;
using Microsoft.OpenApi.Models;
using Moq;
using Moq.AutoMock;
using Shouldly;
using Swagabond.ObjectModelV1;
using Swagabond.ObjectModelV1.Transformer;
using Swagabond.Tests.Extensions;

namespace Swagabond.Tests.ObjectModelV1;

public class ApiV1TransformerTests
{
    [Fact]
    public void FromOpenApi_MapsBasicPropertiesCorrectly()
    {
        // Arrange
        var autoMocker = new AutoMocker();
        
        var pathTransformer = autoMocker.GetMock<IPathV1Transformer>();
        pathTransformer.SetupMockTransformer<IPathV1Transformer, PathV1>();
        
        var infoTransformer = autoMocker.GetMock<IInfoV1Transformer>();
        infoTransformer.SetupMockTransformer<IInfoV1Transformer, InfoV1>();
        
        var externalDocsTransformer = autoMocker.GetMock<IExternalDocsV1Transformer>();
        externalDocsTransformer.SetupMockTransformer<IExternalDocsV1Transformer, HrefV1>();
        
        var schemaDefinitionTransformer = autoMocker.GetMock<ISchemaDefinitionV1Transformer>();
        schemaDefinitionTransformer.SetupMockTransformer<ISchemaDefinitionV1Transformer, SchemaDefinitionV1>();
        
        var extensionTransformer = autoMocker.GetMock<IExtensionV1Transformer>();
        extensionTransformer.SetupMockTransformer<IExtensionV1Transformer, List<ExtensionV1>>();

        // create most of a mock OpenApi Document
        var fixture = new Fixture().WithOpenApiConfigured();
        var openApiDocument = fixture.Create<OpenApiDocument>();

        var metaData = new Dictionary<string, string>()
        {
            { "test1", "value1" },
            { "asdf", "dsaf234" },
            { "test2", "value2" }
        };
        
        var target = autoMocker.CreateInstance<ApiV1Transformer>();

        var result = target.FromOpenApi(new() 
            {
                Metadata = metaData
            },
            openApiDocument, 
            "abc123");

        result.IsEmpty.ShouldBeFalse();
        result.Metadata.ShouldBeEquivalentTo(metaData);

        result.Name.ShouldNotBeEmpty();
        result.Title.ShouldBe(openApiDocument.Info.Title);
        result.Description.ShouldBe(openApiDocument.Info.Description);
        result.Version.ShouldBe(openApiDocument.Info.Version);
        
        result.SpecVersion.ShouldBe("abc123");
    }
}