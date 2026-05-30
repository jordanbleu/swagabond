using Microsoft.OpenApi.Models;
using Moq;
using Moq.AutoMock;
using Shouldly;
using Swagabond.ObjectModelV1;
using Swagabond.ObjectModelV1.Transformer;

namespace Swagabond.Tests.ObjectModelV1;

public class RequestBodyV1TransformerTests
{
    private readonly AutoMocker _mocker = new();
    private readonly RequestBodyV1Transformer _target;
    private readonly PathV1 _path = new() { Route = "/items", Name = "Items", IsEmpty = false };
    private readonly OperationV1 _operation = new() { Method = "Post", IsEmpty = false };

    public RequestBodyV1TransformerTests()
    {
        _mocker.GetMock<ISchemaDefinitionV1Transformer>()
            .Setup(x => x.FromOpenApi(It.IsAny<OpenApiSchema>(), It.IsAny<ApiV1>()))
            .Returns(new SchemaDefinitionV1 { IsEmpty = false });

        _target = _mocker.CreateInstance<RequestBodyV1Transformer>();
    }

    [Fact]
    public void FromOpenApi_AlwaysSetsIsEmptyFalse()
    {
        var body = new OpenApiRequestBody();

        var result = _target.FromOpenApi(body, _operation, _path, new ApiV1());

        result.IsEmpty.ShouldBeFalse();
    }

    [Fact]
    public void FromOpenApi_Name_IsComposedFromRouteAndMethod()
    {
        var body = new OpenApiRequestBody();

        var result = _target.FromOpenApi(body, _operation, _path, new ApiV1());

        // {route.ToClassName()}{method.ToPascalCase().ToClassName()}Request
        result.Name.ShouldBe("ItemsPostRequest");
    }

    [Fact]
    public void FromOpenApi_Title_ContainsMethodAndRoute()
    {
        var body = new OpenApiRequestBody();

        var result = _target.FromOpenApi(body, _operation, _path, new ApiV1());

        result.Title.ShouldBe("POST /items RequestBody");
    }

    [Fact]
    public void FromOpenApi_NoContent_SchemaTransformerNotCalled()
    {
        var body = new OpenApiRequestBody { Content = new Dictionary<string, OpenApiMediaType>() };

        _target.FromOpenApi(body, _operation, _path, new ApiV1());

        _mocker.GetMock<ISchemaDefinitionV1Transformer>()
            .Verify(x => x.FromOpenApi(It.IsAny<OpenApiSchema>(), It.IsAny<ApiV1>()), Times.Never);
    }

    [Fact]
    public void FromOpenApi_WithContent_CallsSchemaTransformer()
    {
        var body = new OpenApiRequestBody
        {
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new OpenApiMediaType { Schema = new OpenApiSchema { Type = "object" } }
            }
        };

        _target.FromOpenApi(body, _operation, _path, new ApiV1());

        _mocker.GetMock<ISchemaDefinitionV1Transformer>()
            .Verify(x => x.FromOpenApi(It.IsAny<OpenApiSchema>(), It.IsAny<ApiV1>()), Times.Once);
    }

    [Fact]
    public void FromOpenApi_BackrefsApiAndOperation()
    {
        var api = new ApiV1();
        var body = new OpenApiRequestBody();

        var result = _target.FromOpenApi(body, _operation, _path, api);

        result.Api.ShouldBeSameAs(api);
        result.Operation.ShouldBeSameAs(_operation);
    }

    [Fact]
    public void FromOpenApi_Description_TakenFromSchemaDescription()
    {
        var body = new OpenApiRequestBody
        {
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema { Description = "The request payload" }
                }
            }
        };

        var result = _target.FromOpenApi(body, _operation, _path, new ApiV1());

        result.Description.ShouldBe("The request payload");
    }
}
