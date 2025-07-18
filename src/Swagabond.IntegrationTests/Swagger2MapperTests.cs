using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Swagabond.Core.Mappers;
using Swagabond.IntegrationTests.Utils;
using Swagabond.ObjectModelV1;

namespace Swagabond.IntegrationTests;

public class Swagger2MapperTestsFixture : IAsyncLifetime
{
    public ApiV1 MappedApi { get; private set; }
    
    public async Task InitializeAsync()
    {
        var apiV1Transformer = ApiTransformerFactory.CreateV1Transformer();

        var mapper = apiV1Transformer.GetRequiredService<OpenApiMapper>();
        var path = Path.Combine(AppContext.BaseDirectory, "SwaggerFiles/Swagger_2_0.json");
        var fs = new FileStream(path, FileMode.Open);
        var mapped = await mapper.MapFromStreamV1(new()
        {
            FailOnDefinitionError = false,
            FailOnDefinitionWarning = false
        }, fs);

        MappedApi = mapped;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}

public class Swagger2MapperTests : IClassFixture<Swagger2MapperTestsFixture>
{
    private Swagger2MapperTestsFixture _fixture;

    public Swagger2MapperTests(Swagger2MapperTestsFixture fixture)
    {
        _fixture = fixture;
    }
    
    
    [Fact]
    [Category("integration_openapi_2")]
    public void TopLevelProperties_AreMappedCorrectly()
    {
        var api = _fixture.MappedApi;
        api.Title.ShouldBe("Swagger Petstore");
        api.Description.ShouldStartWith(
            "This is a sample server Petstore server.");
        api.Extensions.ShouldBeEmpty();
        api.Name.ShouldBe("SwaggerPetstore"); 
        api.Version.ShouldBe("1.0.7");
        api.IsEmpty.ShouldBeFalse();
        api.SpecType.ShouldBe(SpecTypeV1.OpenApi);
        api.SpecVersion.ShouldBe("OpenApi2_0");
    }

    [Fact]
    [Category("integration_openapi_2")]
    public void ApiInfo_IsMappedCorrectly()
    {
        var info = _fixture.MappedApi?.Info;
        info.ShouldNotBeNull();
        info.ContactEmail.ShouldBe("apiteam@swagger.io");
        info.ContactName.ShouldBeEmpty();
        info.ContactUrl.ShouldBeEmpty();
        info.LicenseName.ShouldBe("Apache 2.0");
        info.LicenseUrl.ShouldBe("http://www.apache.org/licenses/LICENSE-2.0.html");
        info.HasContactInfo.ShouldBe(true);
        info.HasContactInfo.ShouldBe(true);
        info.TermsOfServiceUrl.ShouldBe("http://swagger.io/terms/");
    }

    [Fact]
    [Category("integration_openapi_2")]
    public void Servers_AreMappedCorrectly()
    {
        var servers = _fixture.MappedApi?.Servers;
        servers.ShouldNotBeNull();
        servers.Count.ShouldBe(2);
        // I think for 2.0 it derives servers from the 'host' 
        // which is pretty damn neat.
        
        servers.ForEach(s =>
        {
            s.Url.ShouldContain("petstore.swagger.io");
        });
    }

    [Fact]
    [Category("integration_openapi_2")]
    public void ExternalDoc_IsMappedCorrectly()
    {
        var externalDocs = _fixture.MappedApi?.ExternalDocumentationLink;
        externalDocs.ShouldNotBeNull();
        externalDocs.Text.ShouldBe("Find out more about Swagger");
        externalDocs.Url.ShouldBe("http://swagger.io/");
    }

    [Fact]
    [Category("integration_openapi_2")]
    public void Paths_AreMappedCorrectly()
    {
        var paths = _fixture.MappedApi?.Paths;
        paths.ShouldNotBeNull();
        paths.ShouldNotBeEmpty();
        paths.Count.ShouldBe(14);

        // general tests for all paths
        foreach (var path in paths)
        {
            path.IsEmpty.ShouldBeFalse();
            path.Extensions.ShouldBeEmpty();
            path.Route.ShouldNotBeNullOrEmpty();
            path.Title.ShouldNotBeNullOrEmpty();
            path.Name.ShouldNotBeNullOrEmpty();
            path.Operations.ShouldNotBeEmpty();
            path.Description.ShouldNotBeEmpty();
            path.Api.ShouldNotBeNull();
        }
    }

    [Fact]
    [Category("integration_openapi_2")]
    public void Operations_AreMappedCorrectly()
    {
        var operations = _fixture.MappedApi?.Operations?.ToList();
        operations.ShouldNotBeNull();
        operations.ShouldNotBeEmpty();

        foreach (var operation in operations)
        {
            operation.Description.ShouldNotBeNull();
            operation.Extensions.ShouldBeEmpty();
            operation.Api.ShouldNotBeNull();
            operation.Name.ShouldNotBeNullOrEmpty();
            operation.Title.ShouldNotBeNullOrEmpty();
            operation.Method.ShouldNotBeNullOrEmpty();
            operation.IsEmpty.ShouldBeFalse();
            // Ensure nothing is null even if empty
            operation.CookieParameters.ShouldNotBeNull();
            operation.HeaderParameters.ShouldNotBeNull();
            operation.RequestBody.ShouldNotBeNull();
            operation.ResponseBodies.ShouldNotBeNull();
            operation.DefaultResponseBody.ShouldNotBeNull();
            operation.ErrorResponseBody.ShouldNotBeNull();
            operation.SuccessResponseBody.ShouldNotBeNull();
        }
    }

    [Fact]
    [Category("integration_openapi_2")]
    public void Schemas_AreMappedCorrectly()
    {
        var schemas = _fixture.MappedApi?.Schemas;
        schemas.ShouldNotBeNull();
        schemas.ShouldNotBeEmpty();

        foreach (var schema in schemas)
        {
            schema.Description.ShouldNotBeNull();
            schema.Extensions.ShouldBeEmpty();
            schema.Api.ShouldNotBeNull();
            schema.Example.ShouldNotBeNull();
            schema.Name.ShouldNotBeEmpty();
            schema.Properties.ShouldNotBeNull();
            schema.Title.ShouldNotBeNullOrEmpty();
        }
    }
}