using System.ComponentModel;
using Shouldly;
using Swagabond.ObjectModelV1;

namespace Swagabond.IntegrationTests;

/// <summary>
/// Deep spot-checks for specific operations in the swagger 2.0 fixture,
/// providing parity with the swagger 3.0 SpotCheckedOperation test.
/// </summary>
public class Swagger2SpotCheckTests : IClassFixture<Swagger2MapperTestsFixture>
{
    private readonly Swagger2MapperTestsFixture _fixture;

    public Swagger2SpotCheckTests(Swagger2MapperTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    [Category("integration_openapi_2")]
    public void GetPetById_OperationIsMappedCorrectly()
    {
        var operation = _fixture.MappedApi.Operations
            .FirstOrDefault(o => o.Path.Route == "/pet/{petId}" && o.Method == "Get");

        operation.ShouldNotBeNull();
        operation.IsEmpty.ShouldBeFalse();
        operation.Api.ShouldNotBeNull();
        operation.Api.IsEmpty.ShouldBeFalse();

        operation.Description.ShouldNotBeNullOrEmpty();
        operation.Method.ShouldBe("Get");
        operation.Name.ShouldNotBeNullOrEmpty();
        operation.Title.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    [Category("integration_openapi_2")]
    public void GetPetById_PathParameterIsMapped()
    {
        var operation = _fixture.MappedApi.Operations
            .First(o => o.Path.Route == "/pet/{petId}" && o.Method == "Get");

        operation.PathParameters.ShouldNotBeEmpty();
        var petIdParam = operation.PathParameters
            .FirstOrDefault(p => p.OriginalName == "petId");

        petIdParam.ShouldNotBeNull();
        petIdParam.Schema.DataType.ShouldBe(DataTypeV1.Int64);
    }

    [Fact]
    [Category("integration_openapi_2")]
    public void GetPetById_SuccessResponseIsMapped()
    {
        var operation = _fixture.MappedApi.Operations
            .First(o => o.Path.Route == "/pet/{petId}" && o.Method == "Get");

        operation.SuccessResponseBody.ShouldNotBeNull();
        operation.SuccessResponseBody.IsEmpty.ShouldBeFalse();
        operation.SuccessResponseBody.StatusCode.ShouldBe(200);
        operation.SuccessResponseBody.Schema.ShouldNotBeNull();
        operation.SuccessResponseBody.Schema.ReferenceId.ShouldBe("Pet");
        operation.SuccessResponseBody.Schema.Properties.Count.ShouldBe(6);
    }

    [Fact]
    [Category("integration_openapi_2")]
    public void GetPetById_ErrorResponseIsMapped()
    {
        var operation = _fixture.MappedApi.Operations
            .First(o => o.Path.Route == "/pet/{petId}" && o.Method == "Get");

        operation.ErrorResponseBody.ShouldNotBeNull();
        operation.ErrorResponseBody.StatusCode.ShouldBeGreaterThan(299);
    }

    [Fact]
    [Category("integration_openapi_2")]
    public void PlaceOrder_RequestBodyIsMapped()
    {
        var operation = _fixture.MappedApi.Operations
            .FirstOrDefault(o => o.Path.Route == "/store/order" && o.Method == "Post");

        operation.ShouldNotBeNull();
        operation.RequestBody.ShouldNotBeNull();
        operation.RequestBody.IsEmpty.ShouldBeFalse();
        operation.RequestBody.Schema.ShouldNotBeNull();
        operation.RequestBody.Schema.ReferenceId.ShouldBe("Order");
        operation.RequestBody.Schema.Properties.Count.ShouldBe(6);
    }

    [Fact]
    [Category("integration_openapi_2")]
    public void PlaceOrder_SuccessResponseSchemaMatchesRequestSchema()
    {
        var operation = _fixture.MappedApi.Operations
            .First(o => o.Path.Route == "/store/order" && o.Method == "Post");

        operation.SuccessResponseBody.StatusCode.ShouldBe(200);
        operation.SuccessResponseBody.Schema.ReferenceId.ShouldBe("Order");
    }
}
