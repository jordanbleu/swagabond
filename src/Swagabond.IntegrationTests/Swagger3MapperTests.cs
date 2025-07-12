using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Swagabond.Core.Mappers;
using Swagabond.IntegrationTests.Utils;
using Swagabond.ObjectModelV1;

namespace Swagabond.IntegrationTests;

public class Swagger3MapperTestsFixture : IAsyncLifetime
{
    public ApiV1 MappedApi { get; private set; }
    
    public async Task InitializeAsync()
    {
        var apiV1Transformer = ApiTransformerFactory.CreateV1Transformer();

        var mapper = apiV1Transformer.GetRequiredService<OpenApiMapper>();
        var fs = new FileStream("SwaggerFiles/Swagger_3_0_4.json", FileMode.Open);
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

/// <summary>
/// Tests for swagger ~3.0
/// </summary>
public class Swagger3MapperTests : IClassFixture<Swagger3MapperTestsFixture>
{
    private Swagger3MapperTestsFixture _fixture;

    public Swagger3MapperTests(Swagger3MapperTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    [Category("integration_openapi_3")]
    public void TopLevelProperties_AreMappedCorrectly()
    {
        var api = _fixture.MappedApi;
        api.Title.ShouldBe("Swagger Petstore - OpenAPI 3.0");
        api.Description.ShouldStartWith(
            "This is a sample Pet Store Server based on the OpenAPI 3.0 specification.");
        api.Extensions.ShouldBeEmpty();
        api.Name.ShouldBe("SwaggerPetstoreOpenApi30"); 
        api.Version.ShouldBe("1.0.26");
        api.IsEmpty.ShouldBeFalse();
        api.SpecType.ShouldBe(SpecTypeV1.OpenApi);
        api.SpecVersion.ShouldBe("OpenApi3_0");
    }

    [Fact]
    [Category("integration_openapi_3")]
    public void ApiInfo_IsMappedCorrectly()
    {
        var info = _fixture.MappedApi?.Info;
        info.ShouldNotBeNull();
        info.ContactEmail.ShouldBe("apiteam@swagger.io");
        info.ContactName.ShouldBeEmpty();
        info.ContactUrl.ShouldBeEmpty();
        info.LicenseName.ShouldBe("Apache 2.0");
        info.LicenseUrl.ShouldBe("https://www.apache.org/licenses/LICENSE-2.0.html");
        info.HasContactInfo.ShouldBe(true);
        info.HasContactInfo.ShouldBe(true);
        info.TermsOfServiceUrl.ShouldBe("https://swagger.io/terms/");
    }

    [Fact]
    [Category("integration_openapi_3")]
    public void Servers_AreMappedCorrectly()
    {
        var servers = _fixture.MappedApi?.Servers;
        servers.ShouldNotBeNull();
        servers.Count.ShouldBe(1);
        servers.Single().Url.ShouldBe("/api/v3");
    }

    [Fact]
    [Category("integration_openapi_3")]
    public void ExternalDoc_IsMappedCorrectly()
    {
        var externalDocs = _fixture.MappedApi?.ExternalDocumentationLink;
        externalDocs.ShouldNotBeNull();
        externalDocs.Text.ShouldBe("Find out more about Swagger");
        externalDocs.Url.ShouldBe("https://swagger.io/");
    }

    [Fact]
    [Category("integration_openapi_3")]
    public void Paths_AreMappedCorrectly()
    {
        var paths = _fixture.MappedApi?.Paths;
        paths.ShouldNotBeNull();
        paths.ShouldNotBeEmpty();
        paths.Count.ShouldBe(13);

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
    [Category("integration_openapi_3")]
    public void Operations_AreMappedCorrectly()
    {
        var operations = _fixture.MappedApi?.Operations?.ToList();
        operations.ShouldNotBeNull();
        operations.ShouldNotBeEmpty();

        foreach (var operation in operations)
        {
            // The input file always has a description
            operation.Description.ShouldNotBeNullOrEmpty();
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
    [Category("integration_openapi_3")]
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

    [Fact]
    [Category("integration_openapi_3")]
    public void SpotCheckedOperation_IsMappedCorrectly()
    {
        var operation = _fixture.MappedApi?.Operations
            .FirstOrDefault(o => o.Path.Route == "/pet" && o.Method == "Put");

        operation.ShouldNotBeNull();
        operation.IsEmpty.ShouldBeFalse();
        
        operation.Api.ShouldNotBeNull();
        operation.Api.IsEmpty.ShouldBeFalse();

        operation.Description.ShouldBe("Update an existing pet by Id.");
        
        operation.Extensions.ShouldNotBeNull();
        operation.Extensions.ShouldBeEmpty();

        operation.Method.ShouldBe("Put");

        operation.Name.ShouldBe("PutPet");
        
        operation.Path.ShouldNotBeNull();
        operation.Path.IsEmpty.ShouldBeFalse();

        operation.Title.ShouldBe("PUT /pet");

        operation.RequestBody.ShouldNotBeNull();
        operation.RequestBody.IsEmpty.ShouldBeFalse();
        operation.RequestBody.Description.ShouldNotBeNull();
        operation.RequestBody.Api.ShouldNotBeNull();
        operation.RequestBody.Api.IsEmpty.ShouldBeFalse();
        operation.RequestBody.Name.ShouldBe("PetPutRequest");
        operation.RequestBody.Title.ShouldBe("PUT /pet RequestBody");
        operation.RequestBody.Schema.ShouldNotBeNull();
        operation.RequestBody.Schema.Description.ShouldNotBeNull();
        operation.RequestBody.Schema.Example.ShouldNotBeNull();
        operation.RequestBody.Schema.Title.ShouldBe("Pet");
        operation.RequestBody.Schema.Name.ShouldBe("Pet");
        operation.RequestBody.Schema.DataType.ShouldBe(DataTypeV1.Object);
        operation.RequestBody.Schema.EnumOptions.ShouldNotBeNull();
        operation.RequestBody.Schema.EnumOptions.ShouldBeEmpty();
        operation.RequestBody.Schema.IsEnum.ShouldBeFalse();
        operation.RequestBody.Schema.JsonExample.ShouldNotBeNullOrWhiteSpace();
        operation.RequestBody.Schema.IsPrimitive.ShouldBeFalse();
        operation.RequestBody.Schema.OriginalName.ShouldBe("Pet");
        operation.RequestBody.Schema.ReferenceId.ShouldBe("Pet");
        operation.RequestBody.Schema.Properties.Count.ShouldBe(6);

        operation.ResponseBodies.Count.ShouldBe(4);
        operation.SuccessResponseBody.ShouldNotBeNull();
        
        operation.SuccessResponseBody.StatusCode.ShouldBe(200);
        operation.SuccessResponseBody.Name.ShouldBe("Pet200PutResponse");
        operation.SuccessResponseBody.Schema.Properties.Count.ShouldBe(6);
        
        operation.DefaultResponseBody.Description.ShouldBe("Unexpected error"); // api has a fallback error response

        // ErrorResponseBody doesn't make sense for this API (and will be empty) but still shouldn't be null
        operation.ErrorResponseBody.ShouldNotBeNull();
        


    }

}