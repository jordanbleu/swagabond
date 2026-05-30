using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Swagabond.Core.Mappers;
using Swagabond.IntegrationTests.Utils;
using Swagabond.ObjectModelV1;

namespace Swagabond.IntegrationTests;

public class SampleApiMapperTestsFixture : IAsyncLifetime
{
    public ApiV1 MappedApi { get; private set; }

    public async Task InitializeAsync()
    {
        var services = ApiTransformerFactory.CreateV1Transformer();
        var mapper = services.GetRequiredService<OpenApiMapper>();
        var path = Path.Combine(AppContext.BaseDirectory, "SwaggerFiles/swagger-sample.json");
        var fs = new FileStream(path, FileMode.Open);
        MappedApi = await mapper.MapFromStreamV1(new()
        {
            FailOnDefinitionError = false,
            FailOnDefinitionWarning = false
        }, fs);
    }

    public Task DisposeAsync() => Task.CompletedTask;
}

[Category("integration_sample")]
public class SampleApiMapperTests : IClassFixture<SampleApiMapperTestsFixture>
{
    private readonly SampleApiMapperTestsFixture _fixture;

    public SampleApiMapperTests(SampleApiMapperTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void TopLevelProperties_AreMappedCorrectly()
    {
        var api = _fixture.MappedApi;
        api.IsEmpty.ShouldBeFalse();
        api.Title.ShouldBe("SampleWebApi");
        api.Name.ShouldBe("SampleWebApi");
        api.Description.ShouldStartWith("This is a sample web api");
        api.Version.ShouldBe("v1");
        api.SpecVersion.ShouldBe("OpenApi3_0");
        api.SpecType.ShouldBe(SpecTypeV1.OpenApi);
    }

    [Fact]
    public void ApiInfo_IsMappedCorrectly()
    {
        var info = _fixture.MappedApi.Info;
        info.ShouldNotBeNull();
        info.ContactEmail.ShouldBe("test@gmail.com");
        info.ContactName.ShouldBe("Hugh Man");
        info.ContactUrl.ShouldBe("https://www.google.com/");
        info.LicenseName.ShouldBe("MIT");
        info.LicenseUrl.ShouldBe("https://opensource.org/licenses/MIT");
        info.HasContactInfo.ShouldBeTrue();
        info.TermsOfServiceUrl.ShouldBe("https://en.wikipedia.org/wiki/Lorem_ipsum");
    }

    [Fact]
    public void Servers_AreMappedCorrectly()
    {
        var api = _fixture.MappedApi;
        api.Servers.Count.ShouldBe(1);
        api.Servers.Single().Url.ShouldBe("http://localhost:5240");
        api.BaseUrls.ShouldContain("http://localhost:5240");
    }

    [Fact]
    public void ExternalDocs_IsEmpty_WhenNotInSpec()
    {
        var link = _fixture.MappedApi.ExternalDocumentationLink;
        link.IsEmpty.ShouldBeTrue();
        link.Url.ShouldBeEmpty();
        link.Text.ShouldBeEmpty();
    }

    [Fact]
    public void Paths_AreMappedCorrectly()
    {
        var paths = _fixture.MappedApi.Paths;
        paths.Count.ShouldBe(7);
        foreach (var path in paths)
        {
            path.IsEmpty.ShouldBeFalse();
            path.Route.ShouldNotBeNullOrEmpty();
            path.Name.ShouldNotBeNullOrEmpty();
            path.Operations.ShouldNotBeEmpty();
            path.Api.ShouldNotBeNull();
        }
    }

    [Fact]
    public void Schemas_AreMappedCorrectly()
    {
        var schemas = _fixture.MappedApi.Schemas;
        schemas.Count.ShouldBe(12);
        foreach (var schema in schemas)
        {
            schema.IsEmpty.ShouldBeFalse();
            schema.Name.ShouldNotBeNullOrEmpty();
            schema.Api.ShouldNotBeNull();
        }
    }

    [Fact]
    public void SchemaNames_UseDottedNamespaceStripped()
    {
        var schemas = _fixture.MappedApi.Schemas;

        var franchise = schemas.FirstOrDefault(s => s.ReferenceId == "SampleWebApi.Controllers.FranchiseGetResponseItem");
        franchise.ShouldNotBeNull();
        franchise.Name.ShouldBe("SampleWebApiControllersFranchiseGetResponseItem");
        franchise.OriginalName.ShouldBe("SampleWebApi.Controllers.FranchiseGetResponseItem");
    }

    [Fact]
    public void ApiLevelExtensions_AreMapped()
    {
        var api = _fixture.MappedApi;
        api.Extensions.ShouldNotBeEmpty();
        var ext = api.Extensions.FirstOrDefault(e => e.Name == "x-testExt");
        ext.ShouldNotBeNull();
        ext.Value.ShouldBe("hello world!");
    }

    [Fact]
    public void ExtensionDictionary_CanLookUpByKey()
    {
        var api = _fixture.MappedApi;
        api.ExtensionDictionary.ContainsKey("x-testExt").ShouldBeTrue();
        api.ExtensionDictionary["x-testExt"].ShouldBe("hello world!");
    }

    [Fact]
    public void Post_WithCreatedResponse_HasStatusCode201()
    {
        var op = _fixture.MappedApi.Operations
            .FirstOrDefault(o => o.Path.Route == "/api/v1/restaurants" && o.Method == "Post");

        op.ShouldNotBeNull();
        op.SuccessResponseBody.StatusCode.ShouldBe(201);
        op.SuccessResponseBody.Schema.ReferenceId.ShouldBe("SampleWebApi.Controllers.RestaurantGetResponseItem");
    }

    [Fact]
    public void Delete_WithNoContentResponse_HasStatusCode204AndEmptySchema()
    {
        var op = _fixture.MappedApi.Operations
            .FirstOrDefault(o => o.Path.Route == "/api/v1/restaurants/{id}" && o.Method == "Delete");

        op.ShouldNotBeNull();
        op.SuccessResponseBody.StatusCode.ShouldBe(204);
        op.SuccessResponseBody.Schema.IsEmpty.ShouldBeTrue();
    }

    [Fact]
    public void PathParam_WithUuidFormat_IsGuidType()
    {
        var op = _fixture.MappedApi.Operations
            .FirstOrDefault(o => o.Path.Route == "/api/v1/franchises/{id}" && o.Method == "Get");

        op.ShouldNotBeNull();
        op.PathParameters.Count.ShouldBe(1);
        op.PathParameters.Single().OriginalName.ShouldBe("id");
        op.PathParameters.Single().Schema.DataType.ShouldBe(DataTypeV1.Guid);
    }

    [Fact]
    public void NestedObjectSchema_HasPropertiesMapped()
    {
        var fullMenuItem = _fixture.MappedApi.Schemas
            .FirstOrDefault(s => s.ReferenceId == "SampleWebApi.Controllers.FullMenuItemGetResponse");

        fullMenuItem.ShouldNotBeNull();
        fullMenuItem.DataType.ShouldBe(DataTypeV1.Object);
        fullMenuItem.Properties.Count.ShouldBe(2);

        var itemProp = fullMenuItem.Properties.FirstOrDefault(p => p.OriginalName == "item");
        itemProp.ShouldNotBeNull();
        itemProp.Schema.DataType.ShouldBe(DataTypeV1.Object);
        itemProp.Schema.ReferenceId.ShouldBe("SampleWebApi.Controllers.MenuItemResponseItem");

        var franchiseProp = fullMenuItem.Properties.FirstOrDefault(p => p.OriginalName == "franchise");
        franchiseProp.ShouldNotBeNull();
        franchiseProp.Schema.DataType.ShouldBe(DataTypeV1.Object);
        franchiseProp.Schema.ReferenceId.ShouldBe("SampleWebApi.Controllers.FranchiseInformation");
    }

    [Fact]
    public void ArrayPropertySchema_IsMarkedAsArray()
    {
        var restaurantList = _fixture.MappedApi.Schemas
            .FirstOrDefault(s => s.ReferenceId == "SampleWebApi.Controllers.RestaurantGetResponse");

        restaurantList.ShouldNotBeNull();

        var itemsProp = restaurantList.Properties.FirstOrDefault(p => p.OriginalName == "items");
        itemsProp.ShouldNotBeNull();
        itemsProp.Schema.IsArray.ShouldBeTrue();
        itemsProp.Schema.DataType.ShouldBe(DataTypeV1.Object);
    }

    [Fact]
    public void NullableProperty_SetsIsNullableConstraint()
    {
        var franchise = _fixture.MappedApi.Schemas
            .FirstOrDefault(s => s.ReferenceId == "SampleWebApi.Controllers.FranchiseGetResponseItem");

        franchise.ShouldNotBeNull();

        var nameProp = franchise.Properties.FirstOrDefault(p => p.OriginalName == "name");
        nameProp.ShouldNotBeNull();
        nameProp.Schema.Constraints.IsNullable.ShouldBeTrue();

        var idProp = franchise.Properties.FirstOrDefault(p => p.OriginalName == "id");
        idProp.ShouldNotBeNull();
        idProp.Schema.Constraints.IsNullable.ShouldBeFalse();
    }
}
