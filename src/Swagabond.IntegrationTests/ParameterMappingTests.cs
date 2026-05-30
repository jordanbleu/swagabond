using System.ComponentModel;
using Shouldly;
using Swagabond.ObjectModelV1;

namespace Swagabond.IntegrationTests;

/// <summary>
/// Tests for query, path, header, and cookie parameter mapping across all parameter locations.
/// </summary>
[Category("integration_openapi_3")]
public class ParameterMappingTests_OpenApi3 : IClassFixture<Swagger3MapperTestsFixture>
{
    private readonly Swagger3MapperTestsFixture _fixture;

    public ParameterMappingTests_OpenApi3(Swagger3MapperTestsFixture fixture)
    {
        _fixture = fixture;
    }

    private OperationV1 GetOp(string route, string method) =>
        _fixture.MappedApi.Operations.First(o => o.Path.Route == route && o.Method == method);

    [Fact]
    public void QueryParam_IsMappedToQueryParameters()
    {
        var op = GetOp("/pet/findByStatus", "Get");
        op.QueryParameters.Count.ShouldBe(1);
        var param = op.QueryParameters.Single();
        param.OriginalName.ShouldBe("status");
        param.Schema.DataType.ShouldBe(DataTypeV1.String);
        op.PathParameters.ShouldBeEmpty();
        op.HeaderParameters.ShouldBeEmpty();
    }

    [Fact]
    public void MultipleQueryParams_AllMapped()
    {
        var op = GetOp("/user/login", "Get");
        op.QueryParameters.Count.ShouldBe(2);
        op.QueryParameters.Select(p => p.OriginalName).ShouldContain("username");
        op.QueryParameters.Select(p => p.OriginalName).ShouldContain("password");
        op.QueryParameters.ShouldAllBe(p => p.Schema.DataType == DataTypeV1.String);
    }

    [Fact]
    public void PathParam_IsMappedToPathParameters()
    {
        var op = GetOp("/pet/{petId}", "Get");
        op.PathParameters.Count.ShouldBe(1);
        var param = op.PathParameters.Single();
        param.OriginalName.ShouldBe("petId");
        param.Schema.DataType.ShouldBe(DataTypeV1.Int64);
        op.QueryParameters.ShouldBeEmpty();
    }

    [Fact]
    public void HeaderParam_IsMappedToHeaderParameters()
    {
        var op = GetOp("/pet/{petId}", "Delete");
        op.HeaderParameters.Count.ShouldBe(1);
        var header = op.HeaderParameters.Single();
        header.OriginalName.ShouldBe("api_key");
        header.Schema.DataType.ShouldBe(DataTypeV1.String);
    }

    [Fact]
    public void MixedParams_EachLandInCorrectCollection()
    {
        // POST /pet/{petId} has 1 path + 2 query params
        var op = GetOp("/pet/{petId}", "Post");
        op.PathParameters.Count.ShouldBe(1);
        op.QueryParameters.Count.ShouldBe(2);
        op.HeaderParameters.ShouldBeEmpty();
        op.CookieParameters.ShouldBeEmpty();

        op.PathParameters.Single().OriginalName.ShouldBe("petId");
        op.QueryParameters.Select(p => p.OriginalName).ShouldContain("name");
        op.QueryParameters.Select(p => p.OriginalName).ShouldContain("status");
    }

    [Fact]
    public void OperationWithNoParams_AllCollectionsEmpty()
    {
        var op = GetOp("/store/inventory", "Get");
        op.QueryParameters.ShouldBeEmpty();
        op.PathParameters.ShouldBeEmpty();
        op.HeaderParameters.ShouldBeEmpty();
        op.CookieParameters.ShouldBeEmpty();
    }

    [Fact]
    public void DeleteWithHeaderAndPath_BothAreMapped()
    {
        var op = GetOp("/pet/{petId}", "Delete");
        op.HeaderParameters.Count.ShouldBe(1);
        op.PathParameters.Count.ShouldBe(1);
        op.PathParameters.Single().OriginalName.ShouldBe("petId");
        op.PathParameters.Single().Schema.DataType.ShouldBe(DataTypeV1.Int64);
    }

    [Fact]
    public void QueryParam_WithEnum_IsMarkedAsEnum()
    {
        var op = GetOp("/pet/findByStatus", "Get");
        var statusParam = op.QueryParameters.Single();
        statusParam.Schema.IsEnum.ShouldBeTrue();
        statusParam.Schema.EnumOptions.Count.ShouldBe(3);
        statusParam.Schema.EnumOptions.Select(e => e.Value)
            .ShouldBe(new[] { "available", "pending", "sold" }, ignoreOrder: false);
    }
}

[Category("integration_openapi_2")]
public class ParameterMappingTests_OpenApi2 : IClassFixture<Swagger2MapperTestsFixture>
{
    private readonly Swagger2MapperTestsFixture _fixture;

    public ParameterMappingTests_OpenApi2(Swagger2MapperTestsFixture fixture)
    {
        _fixture = fixture;
    }

    private OperationV1 GetOp(string route, string method) =>
        _fixture.MappedApi.Operations.First(o => o.Path.Route == route && o.Method == method);

    [Fact]
    public void QueryParam_IsMappedToQueryParameters()
    {
        var op = GetOp("/user/login", "Get");
        op.QueryParameters.Count.ShouldBe(2);
        op.QueryParameters.Select(p => p.OriginalName).ShouldContain("username");
        op.QueryParameters.Select(p => p.OriginalName).ShouldContain("password");
    }

    [Fact]
    public void PathParam_IsMappedToPathParameters()
    {
        var op = GetOp("/pet/{petId}", "Get");
        op.PathParameters.Count.ShouldBe(1);
        op.PathParameters.Single().OriginalName.ShouldBe("petId");
        op.PathParameters.Single().Schema.DataType.ShouldBe(DataTypeV1.Int64);
    }

    [Fact]
    public void HeaderParam_IsMappedToHeaderParameters()
    {
        var op = GetOp("/pet/{petId}", "Delete");
        op.HeaderParameters.ShouldNotBeEmpty();
        op.HeaderParameters.Select(p => p.OriginalName).ShouldContain("api_key");
    }
}

[Category("integration_sample")]
public class ParameterMappingTests_SampleApi : IClassFixture<SampleApiMapperTestsFixture>
{
    private readonly SampleApiMapperTestsFixture _fixture;

    public ParameterMappingTests_SampleApi(SampleApiMapperTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void PathParam_WithUuidFormat_IsGuidType()
    {
        var op = _fixture.MappedApi.Operations
            .First(o => o.Path.Route == "/api/v1/menuitems/{id}" && o.Method == "Get");

        op.PathParameters.Count.ShouldBe(1);
        var param = op.PathParameters.Single();
        param.OriginalName.ShouldBe("id");
        param.Schema.DataType.ShouldBe(DataTypeV1.Guid);
    }
}
