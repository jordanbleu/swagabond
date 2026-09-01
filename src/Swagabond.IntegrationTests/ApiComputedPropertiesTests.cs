using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Swagabond.Core.Mappers;
using Swagabond.IntegrationTests.Utils;
using Swagabond.ObjectModelV1;

namespace Swagabond.IntegrationTests;

/// <summary>
/// Tests for computed/derived properties on ApiV1: Operations flatmap, BaseUrls, and Metadata passthrough.
/// </summary>
[Category("integration_openapi_3")]
public class ApiComputedPropertiesTests_OpenApi3 : IClassFixture<Swagger3MapperTestsFixture>
{
    private readonly Swagger3MapperTestsFixture _fixture;

    public ApiComputedPropertiesTests_OpenApi3(Swagger3MapperTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Operations_CountMatchesSumOfPathOperations()
    {
        var api = _fixture.MappedApi;
        var expectedCount = api.Paths.Sum(p => p.Operations.Count);
        api.Operations.Count.ShouldBe(expectedCount);
        api.Operations.Count.ShouldBe(19);
    }

    [Fact]
    public void Operations_ContainsOperationsFromAllPaths()
    {
        var api = _fixture.MappedApi;
        // Spot check: operations from different paths all appear in the flat list
        api.Operations.ShouldContain(o => o.Path.Route == "/pet" && o.Method == "Put");
        api.Operations.ShouldContain(o => o.Path.Route == "/store/inventory" && o.Method == "Get");
        api.Operations.ShouldContain(o => o.Path.Route == "/user/{username}" && o.Method == "Delete");
    }

    [Fact]
    public void Operations_IsCached_ReturnsSameReferenceOnRepeatedAccess()
    {
        var api = _fixture.MappedApi;
        var first = api.Operations;
        var second = api.Operations;
        first.ShouldBeSameAs(second);
    }

    [Fact]
    public void BaseUrls_ContainsServerUrls()
    {
        var api = _fixture.MappedApi;
        api.BaseUrls.Count.ShouldBe(1);
        api.BaseUrls.ShouldContain("/api/v3");
    }

    [Fact]
    public void BaseUrls_IsCached_ReturnsSameReferenceOnRepeatedAccess()
    {
        var api = _fixture.MappedApi;
        var first = api.BaseUrls;
        var second = api.BaseUrls;
        first.ShouldBeSameAs(second);
    }

    [Fact]
    public void Operations_EachHasApiBackref()
    {
        foreach (var op in _fixture.MappedApi.Operations)
        {
            op.Api.ShouldNotBeNull();
            op.Api.IsEmpty.ShouldBeFalse();
            op.Path.Api.ShouldNotBeNull();
        }
    }
}

[Category("integration_openapi_2")]
public class ApiComputedPropertiesTests_OpenApi2 : IClassFixture<Swagger2MapperTestsFixture>
{
    private readonly Swagger2MapperTestsFixture _fixture;

    public ApiComputedPropertiesTests_OpenApi2(Swagger2MapperTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Operations_CountMatchesSumOfPathOperations()
    {
        var api = _fixture.MappedApi;
        var expectedCount = api.Paths.Sum(p => p.Operations.Count);
        api.Operations.Count.ShouldBe(expectedCount);
        api.Operations.Count.ShouldBe(20);
    }

    [Fact]
    public void BaseUrls_ContainsServerUrls()
    {
        var api = _fixture.MappedApi;
        api.BaseUrls.Count.ShouldBe(2);
        api.BaseUrls.ShouldAllBe(u => u.Contains("petstore.swagger.io"));
    }
}

/// <summary>
/// Tests that Metadata passed to the mapper request is threaded through to the ApiV1 object.
/// These tests create their own mapper instance rather than using a shared fixture.
/// </summary>
[Category("integration_openapi_3")]
public class MetadataPassthroughTests
{
    [Fact]
    public async Task Metadata_IsPopulatedFromMapperRequest()
    {
        var services = ApiTransformerFactory.CreateV1Transformer();
        var mapper = services.GetRequiredService<OpenApiMapper>();
        var path = Path.Combine(AppContext.BaseDirectory, "SwaggerFiles/swagger_3_0_4.json");
        await using var fs = new FileStream(path, FileMode.Open);

        var api = await mapper.MapFromStreamV1(new()
        {
            FailOnDefinitionError = false,
            FailOnDefinitionWarning = false,
            Metadata = new Dictionary<string, string>
            {
                ["outputNamespace"] = "My.Generated.Api",
                ["author"] = "test-runner"
            }
        }, fs);

        api.Metadata.ShouldContainKey("outputNamespace");
        api.Metadata["outputNamespace"].ShouldBe("My.Generated.Api");
        api.Metadata["author"].ShouldBe("test-runner");
    }

    [Fact]
    public async Task EmptyMetadata_IsEmptyDictionary()
    {
        var services = ApiTransformerFactory.CreateV1Transformer();
        var mapper = services.GetRequiredService<OpenApiMapper>();
        var path = Path.Combine(AppContext.BaseDirectory, "SwaggerFiles/swagger_3_0_4.json");
        await using var fs = new FileStream(path, FileMode.Open);

        var api = await mapper.MapFromStreamV1(new()
        {
            FailOnDefinitionError = false,
            FailOnDefinitionWarning = false
        }, fs);

        api.Metadata.ShouldNotBeNull();
        api.Metadata.ShouldBeEmpty();
    }
}
