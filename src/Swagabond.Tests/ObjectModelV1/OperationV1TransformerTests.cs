using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Moq;
using Moq.AutoMock;
using Shouldly;
using Swagabond.ObjectModelV1;
using Swagabond.ObjectModelV1.Transformer;

namespace Swagabond.Tests.ObjectModelV1;

public class OperationV1TransformerTests
{
    private readonly AutoMocker _mocker = new();
    private readonly OperationV1Transformer _target;
    private readonly PathV1 _path = new() { Name = "Items", Route = "/items", IsEmpty = false };

    public OperationV1TransformerTests()
    {
        _mocker.GetMock<IExtensionV1Transformer>()
            .Setup(x => x.FromOpenApi(It.IsAny<IDictionary<string, IOpenApiExtension>>()))
            .Returns(new List<ExtensionV1>());

        _mocker.GetMock<ISchemaDefinitionV1Transformer>()
            .Setup(x => x.FromOpenApi(It.IsAny<OpenApiSchema>(), It.IsAny<ApiV1>()))
            .Returns(new SchemaDefinitionV1 { IsEmpty = false });

        _mocker.GetMock<ISchemaReferenceV1Transformer>()
            .Setup(x => x.FromOpenApi(It.IsAny<string>(), It.IsAny<SchemaDefinitionV1>(), It.IsAny<ApiV1>()))
            .Returns(new SchemaReferenceV1 { IsEmpty = false });

        _mocker.GetMock<IRequestBodyV1Transformer>()
            .Setup(x => x.FromOpenApi(It.IsAny<OpenApiRequestBody>(), It.IsAny<OperationV1>(), It.IsAny<PathV1>(), It.IsAny<ApiV1>()))
            .Returns(new RequestBodyV1 { IsEmpty = false });

        _mocker.GetMock<IResponseBodyV1Transformer>()
            .Setup(x => x.FromOpenApi(It.IsAny<KeyValuePair<string, OpenApiResponse>>(), It.IsAny<ApiV1>(), It.IsAny<OperationV1>()))
            .Returns(new ResponseBodyV1 { IsEmpty = false, StatusCode = 200 });

        _target = _mocker.CreateInstance<OperationV1Transformer>();
    }

    [Fact]
    public void FromOpenApi_SetsIsEmptyFalse()
    {
        var kvp = MakeOperation(OperationType.Get);

        var result = _target.FromOpenApi(kvp, _path, new ApiV1());

        result.IsEmpty.ShouldBeFalse();
    }

    [Theory]
    [InlineData(OperationType.Get, "Get", "GetItems")]
    [InlineData(OperationType.Post, "Post", "PostItems")]
    [InlineData(OperationType.Put, "Put", "PutItems")]
    [InlineData(OperationType.Delete, "Delete", "DeleteItems")]
    public void FromOpenApi_Name_IsMethodPascalCasePlusPathName(OperationType type, string expectedMethod, string expectedName)
    {
        var kvp = MakeOperation(type);

        var result = _target.FromOpenApi(kvp, _path, new ApiV1());

        result.Method.ShouldBe(expectedMethod);
        result.Name.ShouldBe(expectedName);
    }

    [Fact]
    public void FromOpenApi_Title_IsUppercaseMethodAndRoute()
    {
        var kvp = MakeOperation(OperationType.Get);

        var result = _target.FromOpenApi(kvp, _path, new ApiV1());

        result.Title.ShouldBe("GET /items");
    }

    [Fact]
    public void FromOpenApi_Description_TakenFromOperation()
    {
        var kvp = new KeyValuePair<OperationType, OpenApiOperation>(
            OperationType.Get,
            new OpenApiOperation { Description = "Fetch all items" });

        var result = _target.FromOpenApi(kvp, _path, new ApiV1());

        result.Description.ShouldBe("Fetch all items");
    }

    [Fact]
    public void FromOpenApi_BackrefsPathAndApi()
    {
        var api = new ApiV1();
        var kvp = MakeOperation(OperationType.Get);

        var result = _target.FromOpenApi(kvp, _path, api);

        result.Path.ShouldBeSameAs(_path);
        result.Api.ShouldBeSameAs(api);
    }

    [Fact]
    public void FromOpenApi_QueryParameter_LandsInQueryParameters()
    {
        var kvp = MakeOperationWithParam(ParameterLocation.Query, "filter");

        var result = _target.FromOpenApi(kvp, _path, new ApiV1());

        result.QueryParameters.Count.ShouldBe(1);
        result.PathParameters.ShouldBeEmpty();
        result.HeaderParameters.ShouldBeEmpty();
        result.CookieParameters.ShouldBeEmpty();
    }

    [Fact]
    public void FromOpenApi_PathParameter_LandsInPathParameters()
    {
        var kvp = MakeOperationWithParam(ParameterLocation.Path, "id");

        var result = _target.FromOpenApi(kvp, _path, new ApiV1());

        result.PathParameters.Count.ShouldBe(1);
        result.QueryParameters.ShouldBeEmpty();
        result.HeaderParameters.ShouldBeEmpty();
        result.CookieParameters.ShouldBeEmpty();
    }

    [Fact]
    public void FromOpenApi_HeaderParameter_LandsInHeaderParameters()
    {
        var kvp = MakeOperationWithParam(ParameterLocation.Header, "X-Request-Id");

        var result = _target.FromOpenApi(kvp, _path, new ApiV1());

        result.HeaderParameters.Count.ShouldBe(1);
        result.QueryParameters.ShouldBeEmpty();
        result.PathParameters.ShouldBeEmpty();
        result.CookieParameters.ShouldBeEmpty();
    }

    [Fact]
    public void FromOpenApi_CookieParameter_LandsInCookieParameters()
    {
        var kvp = MakeOperationWithParam(ParameterLocation.Cookie, "session");

        var result = _target.FromOpenApi(kvp, _path, new ApiV1());

        result.CookieParameters.Count.ShouldBe(1);
        result.QueryParameters.ShouldBeEmpty();
        result.PathParameters.ShouldBeEmpty();
        result.HeaderParameters.ShouldBeEmpty();
    }

    [Fact]
    public void FromOpenApi_MultipleParamsOfDifferentKinds_EachGoToCorrectList()
    {
        var op = new OpenApiOperation
        {
            Parameters = new List<OpenApiParameter>
            {
                new() { In = ParameterLocation.Query, Name = "q", Schema = new OpenApiSchema() },
                new() { In = ParameterLocation.Path, Name = "id", Schema = new OpenApiSchema() },
                new() { In = ParameterLocation.Header, Name = "X-Trace", Schema = new OpenApiSchema() },
                new() { In = ParameterLocation.Cookie, Name = "sess", Schema = new OpenApiSchema() },
            }
        };
        var kvp = new KeyValuePair<OperationType, OpenApiOperation>(OperationType.Get, op);

        var result = _target.FromOpenApi(kvp, _path, new ApiV1());

        result.QueryParameters.Count.ShouldBe(1);
        result.PathParameters.Count.ShouldBe(1);
        result.HeaderParameters.Count.ShouldBe(1);
        result.CookieParameters.Count.ShouldBe(1);
    }

    [Fact]
    public void FromOpenApi_NoRequestBody_RequestBodyRemainsEmpty()
    {
        var kvp = MakeOperation(OperationType.Get);

        var result = _target.FromOpenApi(kvp, _path, new ApiV1());

        result.RequestBody.IsEmpty.ShouldBeTrue();
        _mocker.GetMock<IRequestBodyV1Transformer>()
            .Verify(x => x.FromOpenApi(It.IsAny<OpenApiRequestBody>(), It.IsAny<OperationV1>(), It.IsAny<PathV1>(), It.IsAny<ApiV1>()), Times.Never);
    }

    [Fact]
    public void FromOpenApi_WithRequestBody_CallsRequestBodyTransformer()
    {
        var op = new OpenApiOperation { RequestBody = new OpenApiRequestBody() };
        var kvp = new KeyValuePair<OperationType, OpenApiOperation>(OperationType.Post, op);

        var result = _target.FromOpenApi(kvp, _path, new ApiV1());

        result.RequestBody.IsEmpty.ShouldBeFalse();
        _mocker.GetMock<IRequestBodyV1Transformer>()
            .Verify(x => x.FromOpenApi(It.IsAny<OpenApiRequestBody>(), It.IsAny<OperationV1>(), It.IsAny<PathV1>(), It.IsAny<ApiV1>()), Times.Once);
    }

    [Fact]
    public void FromOpenApi_NoResponses_ResponseBodiesEmpty()
    {
        var kvp = MakeOperation(OperationType.Get);

        var result = _target.FromOpenApi(kvp, _path, new ApiV1());

        result.ResponseBodies.ShouldBeEmpty();
        result.DefaultResponseBody.IsEmpty.ShouldBeTrue();
    }

    [Fact]
    public void FromOpenApi_DefaultResponse_SetsDefaultResponseBody_NotInResponseBodies()
    {
        var defaultBody = new ResponseBodyV1 { IsEmpty = false, StatusCode = 0, ResponseId = "default" };
        _mocker.GetMock<IResponseBodyV1Transformer>()
            .Setup(x => x.FromOpenApi(
                It.Is<KeyValuePair<string, OpenApiResponse>>(r => r.Key == "default"),
                It.IsAny<ApiV1>(),
                It.IsAny<OperationV1>()))
            .Returns(defaultBody);

        var op = new OpenApiOperation
        {
            Responses = new OpenApiResponses
            {
                ["default"] = new OpenApiResponse { Description = "Error" }
            }
        };
        var kvp = new KeyValuePair<OperationType, OpenApiOperation>(OperationType.Get, op);

        var result = _target.FromOpenApi(kvp, _path, new ApiV1());

        result.DefaultResponseBody.ShouldBeSameAs(defaultBody);
        result.ResponseBodies.ShouldBeEmpty();
    }

    [Fact]
    public void FromOpenApi_NonDefaultResponses_AreInResponseBodies()
    {
        var op = new OpenApiOperation
        {
            Responses = new OpenApiResponses
            {
                ["200"] = new OpenApiResponse(),
                ["404"] = new OpenApiResponse()
            }
        };
        var kvp = new KeyValuePair<OperationType, OpenApiOperation>(OperationType.Get, op);

        var result = _target.FromOpenApi(kvp, _path, new ApiV1());

        result.ResponseBodies.Count.ShouldBe(2);
    }

    [Fact]
    public void FromOpenApi_NoDefaultKey_DefaultResponseBodyFallsBackToSuccessResponse()
    {
        var successBody = new ResponseBodyV1 { IsEmpty = false, StatusCode = 200 };
        _mocker.GetMock<IResponseBodyV1Transformer>()
            .Setup(x => x.FromOpenApi(It.IsAny<KeyValuePair<string, OpenApiResponse>>(), It.IsAny<ApiV1>(), It.IsAny<OperationV1>()))
            .Returns(successBody);

        var op = new OpenApiOperation
        {
            Responses = new OpenApiResponses
            {
                ["200"] = new OpenApiResponse()
            }
        };
        var kvp = new KeyValuePair<OperationType, OpenApiOperation>(OperationType.Get, op);

        var result = _target.FromOpenApi(kvp, _path, new ApiV1());

        result.DefaultResponseBody.ShouldBeSameAs(successBody);
    }

    [Fact]
    public void FromOpenApi_MixedResponses_DefaultKeyGoesToDefaultResponseBody_OthersToResponseBodies()
    {
        var defaultBody = new ResponseBodyV1 { IsEmpty = false, StatusCode = 0, ResponseId = "default" };
        var okBody = new ResponseBodyV1 { IsEmpty = false, StatusCode = 200 };

        _mocker.GetMock<IResponseBodyV1Transformer>()
            .Setup(x => x.FromOpenApi(
                It.Is<KeyValuePair<string, OpenApiResponse>>(r => r.Key == "default"),
                It.IsAny<ApiV1>(), It.IsAny<OperationV1>()))
            .Returns(defaultBody);
        _mocker.GetMock<IResponseBodyV1Transformer>()
            .Setup(x => x.FromOpenApi(
                It.Is<KeyValuePair<string, OpenApiResponse>>(r => r.Key == "200"),
                It.IsAny<ApiV1>(), It.IsAny<OperationV1>()))
            .Returns(okBody);

        var op = new OpenApiOperation
        {
            Responses = new OpenApiResponses
            {
                ["200"] = new OpenApiResponse(),
                ["default"] = new OpenApiResponse()
            }
        };
        var kvp = new KeyValuePair<OperationType, OpenApiOperation>(OperationType.Get, op);

        var result = _target.FromOpenApi(kvp, _path, new ApiV1());

        result.DefaultResponseBody.ShouldBeSameAs(defaultBody);
        result.ResponseBodies.Count.ShouldBe(1);
        result.ResponseBodies[0].ShouldBeSameAs(okBody);
    }

    private static KeyValuePair<OperationType, OpenApiOperation> MakeOperation(OperationType type) =>
        new(type, new OpenApiOperation());

    private static KeyValuePair<OperationType, OpenApiOperation> MakeOperationWithParam(ParameterLocation location, string name) =>
        new(OperationType.Get, new OpenApiOperation
        {
            Parameters = new List<OpenApiParameter>
            {
                new() { In = location, Name = name, Schema = new OpenApiSchema() }
            }
        });
}
