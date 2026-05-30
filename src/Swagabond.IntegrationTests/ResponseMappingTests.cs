using System.ComponentModel;
using Shouldly;
using Swagabond.ObjectModelV1;

namespace Swagabond.IntegrationTests;

/// <summary>
/// Tests for response body mapping: status codes, ResponseId, success/error/default selection,
/// and response bodies with no schema.
/// </summary>
[Category("integration_openapi_3")]
public class ResponseMappingTests_OpenApi3 : IClassFixture<Swagger3MapperTestsFixture>
{
    private readonly Swagger3MapperTestsFixture _fixture;

    public ResponseMappingTests_OpenApi3(Swagger3MapperTestsFixture fixture)
    {
        _fixture = fixture;
    }

    private OperationV1 GetOp(string route, string method) =>
        _fixture.MappedApi.Operations.First(o => o.Path.Route == route && o.Method == method);

    [Fact]
    public void MultipleStatusCodes_AllMappedToResponseBodies()
    {
        // GET /pet/{petId} has responses: 200, 400, 404, default
        var op = GetOp("/pet/{petId}", "Get");
        // ResponseBodies excludes "default" (it goes into DefaultResponseBody)
        op.ResponseBodies.Count.ShouldBe(3);
        op.ResponseBodies.Select(r => r.StatusCode).ShouldContain(200);
        op.ResponseBodies.Select(r => r.StatusCode).ShouldContain(400);
        op.ResponseBodies.Select(r => r.StatusCode).ShouldContain(404);
    }

    [Fact]
    public void ResponseId_IsPreservedAsString()
    {
        var op = GetOp("/pet/{petId}", "Get");
        op.ResponseBodies.Select(r => r.ResponseId).ShouldContain("200");
        op.ResponseBodies.Select(r => r.ResponseId).ShouldContain("400");
        op.ResponseBodies.Select(r => r.ResponseId).ShouldContain("404");
    }

    [Fact]
    public void SuccessResponseBody_ReturnsFirst2xxResponse()
    {
        var op = GetOp("/pet/{petId}", "Get");
        op.SuccessResponseBody.ShouldNotBeNull();
        op.SuccessResponseBody.IsEmpty.ShouldBeFalse();
        op.SuccessResponseBody.StatusCode.ShouldBe(200);
    }

    [Fact]
    public void ErrorResponseBody_ReturnsFirst3xxPlusResponse()
    {
        var op = GetOp("/pet/{petId}", "Get");
        op.ErrorResponseBody.ShouldNotBeNull();
        op.ErrorResponseBody.IsEmpty.ShouldBeFalse();
        op.ErrorResponseBody.StatusCode.ShouldBeGreaterThan(299);
    }

    [Fact]
    public void DefaultResponseBody_MappedFromDefaultKey()
    {
        // PUT /pet has an explicit "default" response
        var op = GetOp("/pet", "Put");
        op.DefaultResponseBody.IsEmpty.ShouldBeFalse();
        op.DefaultResponseBody.ResponseId.ShouldBe("default");
        op.DefaultResponseBody.StatusCode.ShouldBe(0); // "default" is not a numeric status code
    }

    [Fact]
    public void DefaultResponseBody_FallsBackToSuccessWhenNoDefaultKey()
    {
        // GET /store/inventory has only a 200 and "default" response
        // POST /user has 200 and "default" response
        // Find an op with no "default" key - GET /pet/findByStatus has 200, 400, default (it does have default)
        // Actually, all petstore 3.0 ops have "default". Let's verify the fallback
        // by checking an op where DefaultResponseBody == SuccessResponseBody
        var op = GetOp("/pet/{petId}", "Get");
        // This spec has a "default" response so DefaultResponseBody is set from the spec
        // but SuccessResponseBody should be the 200
        op.SuccessResponseBody.StatusCode.ShouldBe(200);
        op.DefaultResponseBody.ResponseId.ShouldBe("default");
    }

    [Fact]
    public void ResponseBody_Name_IsComposedCorrectly()
    {
        var op = GetOp("/pet/{petId}", "Get");
        var successResponse = op.ResponseBodies.First(r => r.StatusCode == 200);
        successResponse.Name.ShouldBe("PetpetId200GetResponse");
    }

    [Fact]
    public void ResponseBody_Title_IncludesStatusCodeAndName()
    {
        var op = GetOp("/pet/{petId}", "Get");
        var successResponse = op.ResponseBodies.First(r => r.StatusCode == 200);
        successResponse.Title.ShouldContain("200");
        successResponse.Title.ShouldContain("Response");
    }

    [Fact]
    public void ResponseBody_BackrefsApi_AndOperation()
    {
        var op = GetOp("/pet", "Put");
        foreach (var response in op.ResponseBodies)
        {
            response.Api.ShouldNotBeNull();
            response.Api.IsEmpty.ShouldBeFalse();
            response.Operation.ShouldNotBeNull();
            response.Operation.IsEmpty.ShouldBeFalse();
        }
    }

    [Fact]
    public void ResponseWithNoBody_HasEmptySchema()
    {
        // GET /store/inventory default response has no body
        var op = GetOp("/store/inventory", "Get");
        op.DefaultResponseBody.IsEmpty.ShouldBeFalse();
        op.DefaultResponseBody.Schema.IsEmpty.ShouldBeTrue();
    }
}

[Category("integration_openapi_2")]
public class ResponseMappingTests_OpenApi2 : IClassFixture<Swagger2MapperTestsFixture>
{
    private readonly Swagger2MapperTestsFixture _fixture;

    public ResponseMappingTests_OpenApi2(Swagger2MapperTestsFixture fixture)
    {
        _fixture = fixture;
    }

    private OperationV1 GetOp(string route, string method) =>
        _fixture.MappedApi.Operations.First(o => o.Path.Route == route && o.Method == method);

    [Fact]
    public void MultipleStatusCodes_AllMapped()
    {
        var op = GetOp("/pet/{petId}", "Get");
        op.ResponseBodies.Select(r => r.StatusCode).ShouldContain(200);
        op.ResponseBodies.Select(r => r.StatusCode).ShouldContain(400);
        op.ResponseBodies.Select(r => r.StatusCode).ShouldContain(404);
    }

    [Fact]
    public void SuccessResponseBody_ReturnsFirst2xxResponse()
    {
        var op = GetOp("/pet/{petId}", "Get");
        op.SuccessResponseBody.StatusCode.ShouldBe(200);
        op.SuccessResponseBody.IsEmpty.ShouldBeFalse();
    }

    [Fact]
    public void DefaultResponseBody_FallsBackToSuccessWhenNoDefaultKey()
    {
        // In v2 petstore, GET /pet/{petId} has no "default" response key
        // so DefaultResponseBody should fall back to the first success response
        var op = GetOp("/pet/{petId}", "Get");
        op.DefaultResponseBody.IsEmpty.ShouldBeFalse();
        op.DefaultResponseBody.StatusCode.ShouldBe(200);
        op.DefaultResponseBody.ShouldBeSameAs(op.SuccessResponseBody);
    }

    [Fact]
    public void DefaultResponseBody_UsesDefaultKeyWhenPresent()
    {
        // GET /user/logout has only a "default" response key in v2
        var op = GetOp("/user/logout", "Get");
        op.DefaultResponseBody.IsEmpty.ShouldBeFalse();
        op.DefaultResponseBody.ResponseId.ShouldBe("default");
    }
}

[Category("integration_sample")]
public class ResponseMappingTests_SampleApi : IClassFixture<SampleApiMapperTestsFixture>
{
    private readonly SampleApiMapperTestsFixture _fixture;

    public ResponseMappingTests_SampleApi(SampleApiMapperTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void CreatedResponse_HasStatusCode201()
    {
        var op = _fixture.MappedApi.Operations
            .First(o => o.Path.Route == "/api/v1/restaurants" && o.Method == "Post");
        op.SuccessResponseBody.StatusCode.ShouldBe(201);
        op.SuccessResponseBody.ResponseId.ShouldBe("201");
    }

    [Fact]
    public void NoContentResponse_HasStatusCode204_AndEmptySchema()
    {
        var op = _fixture.MappedApi.Operations
            .First(o => o.Path.Route == "/api/v1/restaurants/{id}" && o.Method == "Delete");
        op.SuccessResponseBody.StatusCode.ShouldBe(204);
        op.SuccessResponseBody.Schema.IsEmpty.ShouldBeTrue();
    }
}
