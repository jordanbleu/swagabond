using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Moq;
using Moq.AutoMock;
using Shouldly;
using Swagabond.ObjectModelV1;
using Swagabond.ObjectModelV1.Transformer;

namespace Swagabond.Tests.ObjectModelV1;

public class PathV1TransformerTests
{
    private readonly AutoMocker _mocker = new();
    private readonly PathV1Transformer _target;

    public PathV1TransformerTests()
    {
        _mocker.GetMock<IExtensionV1Transformer>()
            .Setup(x => x.FromOpenApi(It.IsAny<IDictionary<string, IOpenApiExtension>>()))
            .Returns(new List<ExtensionV1>());

        _mocker.GetMock<IOperationV1Transformer>()
            .Setup(x => x.FromOpenApi(
                It.IsAny<KeyValuePair<OperationType, OpenApiOperation>>(),
                It.IsAny<PathV1>(),
                It.IsAny<ApiV1>()))
            .Returns(new OperationV1 { IsEmpty = false });

        _target = _mocker.CreateInstance<PathV1Transformer>();
    }

    [Fact]
    public void FromOpenApi_SetsIsEmptyFalse()
    {
        var kvp = MakePath("/items");

        var result = _target.FromOpenApi(kvp, new ApiV1());

        result.IsEmpty.ShouldBeFalse();
    }

    [Fact]
    public void FromOpenApi_Route_IsPreservedExactly()
    {
        var kvp = MakePath("/api/v1/pets/{id}");

        var result = _target.FromOpenApi(kvp, new ApiV1());

        result.Route.ShouldBe("/api/v1/pets/{id}");
    }

    [Fact]
    public void FromOpenApi_Name_IsToClassNameOfRoute()
    {
        var kvp = MakePath("/items");

        var result = _target.FromOpenApi(kvp, new ApiV1());

        result.Name.ShouldBe("Items");
    }

    [Fact]
    public void FromOpenApi_Title_IsTheRoute()
    {
        var kvp = MakePath("/items");

        var result = _target.FromOpenApi(kvp, new ApiV1());

        result.Title.ShouldBe("/items");
    }

    [Fact]
    public void FromOpenApi_Operations_AreAllMapped()
    {
        var pathItem = new OpenApiPathItem
        {
            Operations = new Dictionary<OperationType, OpenApiOperation>
            {
                [OperationType.Get] = new OpenApiOperation(),
                [OperationType.Post] = new OpenApiOperation()
            }
        };
        var kvp = new KeyValuePair<string, OpenApiPathItem>("/items", pathItem);

        var result = _target.FromOpenApi(kvp, new ApiV1());

        result.Operations.Count.ShouldBe(2);
        _mocker.GetMock<IOperationV1Transformer>()
            .Verify(x => x.FromOpenApi(
                It.IsAny<KeyValuePair<OperationType, OpenApiOperation>>(),
                It.IsAny<PathV1>(),
                It.IsAny<ApiV1>()), Times.Exactly(2));
    }

    [Fact]
    public void FromOpenApi_PathWithNoOperations_HasEmptyOperationsList()
    {
        var kvp = MakePath("/items");

        var result = _target.FromOpenApi(kvp, new ApiV1());

        result.Operations.ShouldBeEmpty();
    }

    [Fact]
    public void FromOpenApi_BackrefsApi()
    {
        var api = new ApiV1();
        var kvp = MakePath("/items");

        var result = _target.FromOpenApi(kvp, api);

        result.Api.ShouldBeSameAs(api);
    }

    [Fact]
    public void FromOpenApi_Description_DefaultsToRouteWhenPathDescriptionIsNull()
    {
        var kvp = MakePath("/items");

        var result = _target.FromOpenApi(kvp, new ApiV1());

        result.Description.ShouldBe("/items");
    }

    [Fact]
    public void FromOpenApi_Description_UsesPathDescription_WhenSet()
    {
        var pathItem = new OpenApiPathItem { Description = "Manage items" };
        var kvp = new KeyValuePair<string, OpenApiPathItem>("/items", pathItem);

        var result = _target.FromOpenApi(kvp, new ApiV1());

        result.Description.ShouldBe("Manage items");
    }

    private static KeyValuePair<string, OpenApiPathItem> MakePath(string route) =>
        new(route, new OpenApiPathItem());
}
