using Microsoft.OpenApi.Models;
using Moq;
using Moq.AutoMock;
using Shouldly;
using Swagabond.ObjectModelV1;
using Swagabond.ObjectModelV1.Transformer;

namespace Swagabond.Tests.ObjectModelV1;

public class ResponseBodyV1TransformerTests
{
    private readonly AutoMocker _mocker = new();
    private readonly ResponseBodyV1Transformer _target;
    private readonly PathV1 _path = new() { Name = "Items", Route = "/items", IsEmpty = false };
    private readonly OperationV1 _operation;

    public ResponseBodyV1TransformerTests()
    {
        _mocker.GetMock<ISchemaDefinitionV1Transformer>()
            .Setup(x => x.FromOpenApi(It.IsAny<OpenApiSchema>(), It.IsAny<ApiV1>()))
            .Returns(new SchemaDefinitionV1 { IsEmpty = false });

        _operation = new OperationV1 { Method = "Get", Path = _path, IsEmpty = false };
        _target = _mocker.CreateInstance<ResponseBodyV1Transformer>();
    }

    [Theory]
    [InlineData("200", 200)]
    [InlineData("201", 201)]
    [InlineData("400", 400)]
    [InlineData("404", 404)]
    [InlineData("500", 500)]
    public void FromOpenApi_NumericKey_SetsStatusCode(string key, int expectedCode)
    {
        var response = new KeyValuePair<string, OpenApiResponse>(key, new OpenApiResponse { Description = "desc" });

        var result = _target.FromOpenApi(response, new ApiV1(), _operation);

        result.StatusCode.ShouldBe(expectedCode);
        result.ResponseId.ShouldBe(key);
    }

    [Fact]
    public void FromOpenApi_DefaultKey_StatusCodeIsZero()
    {
        var response = new KeyValuePair<string, OpenApiResponse>("default", new OpenApiResponse { Description = "Error" });

        var result = _target.FromOpenApi(response, new ApiV1(), _operation);

        result.StatusCode.ShouldBe(0);
        result.ResponseId.ShouldBe("default");
    }

    [Fact]
    public void FromOpenApi_AlwaysSetsIsEmptyFalse()
    {
        var response = new KeyValuePair<string, OpenApiResponse>("200", new OpenApiResponse());

        var result = _target.FromOpenApi(response, new ApiV1(), _operation);

        result.IsEmpty.ShouldBeFalse();
    }

    [Fact]
    public void FromOpenApi_NoContent_ReturnsWithoutCallingSchemaTransformer()
    {
        var response = new KeyValuePair<string, OpenApiResponse>("204", new OpenApiResponse { Description = "No Content" });

        var result = _target.FromOpenApi(response, new ApiV1(), _operation);

        result.Schema.IsEmpty.ShouldBeTrue();
        _mocker.GetMock<ISchemaDefinitionV1Transformer>()
            .Verify(x => x.FromOpenApi(It.IsAny<OpenApiSchema>(), It.IsAny<ApiV1>()), Times.Never);
    }

    [Fact]
    public void FromOpenApi_WithContent_CallsSchemaTransformer()
    {
        var response = new KeyValuePair<string, OpenApiResponse>("200", new OpenApiResponse
        {
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new OpenApiMediaType { Schema = new OpenApiSchema { Type = "object" } }
            }
        });

        _target.FromOpenApi(response, new ApiV1(), _operation);

        _mocker.GetMock<ISchemaDefinitionV1Transformer>()
            .Verify(x => x.FromOpenApi(It.IsAny<OpenApiSchema>(), It.IsAny<ApiV1>()), Times.Once);
    }

    [Fact]
    public void FromOpenApi_Name_IsComposedFromPathMethodAndStatusCode()
    {
        var response = new KeyValuePair<string, OpenApiResponse>("200", new OpenApiResponse());

        var result = _target.FromOpenApi(response, new ApiV1(), _operation);

        // Name = {path.Name.ToPascalCase()}{key.ToPascalCase()}{method.ToPascalCase()}Response
        result.Name.ShouldBe("Items200GetResponse");
    }

    [Fact]
    public void FromOpenApi_Title_ContainsStatusCodeAndName()
    {
        var response = new KeyValuePair<string, OpenApiResponse>("200", new OpenApiResponse());

        var result = _target.FromOpenApi(response, new ApiV1(), _operation);

        result.Title.ShouldContain("200");
        result.Title.ShouldContain("Response");
    }

    [Fact]
    public void FromOpenApi_BackrefsApiAndOperation()
    {
        var api = new ApiV1();
        var response = new KeyValuePair<string, OpenApiResponse>("200", new OpenApiResponse());

        var result = _target.FromOpenApi(response, api, _operation);

        result.Api.ShouldBeSameAs(api);
        result.Operation.ShouldBeSameAs(_operation);
    }

    [Fact]
    public void FromOpenApi_Description_CopiedFromResponse()
    {
        var response = new KeyValuePair<string, OpenApiResponse>("200", new OpenApiResponse { Description = "All good" });

        var result = _target.FromOpenApi(response, new ApiV1(), _operation);

        result.Description.ShouldBe("All good");
    }
}
