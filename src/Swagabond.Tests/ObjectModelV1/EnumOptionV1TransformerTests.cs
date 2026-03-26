using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Shouldly;
using Swagabond.ObjectModelV1.Transformer;

namespace Swagabond.Tests.ObjectModelV1;

public class EnumOptionV1TransformerTests
{
    private readonly EnumOptionV1Transformer _target = new();

    [Fact]
    public void FromOpenApi_EmptyValues_ReturnsEmptyList()
    {
        var result = _target.FromOpenApi(
            new List<IOpenApiAny>(),
            new Dictionary<string, IOpenApiExtension>());

        result.ShouldBeEmpty();
    }

    [Fact]
    public void FromOpenApi_ValuesWithoutNames_GeneratesItemNNames()
    {
        var values = new List<IOpenApiAny>
        {
            new OpenApiString("available"),
            new OpenApiString("pending"),
            new OpenApiString("sold")
        };

        var result = _target.FromOpenApi(values, new Dictionary<string, IOpenApiExtension>());

        result.Count.ShouldBe(3);
        result[0].Name.ShouldBe("Item0");
        result[0].Value.ShouldBe("available");
        result[1].Name.ShouldBe("Item1");
        result[2].Name.ShouldBe("Item2");
        result[2].Value.ShouldBe("sold");
    }

    [Theory]
    [InlineData("x-enumNames")]
    [InlineData("x-enum-varnames")]
    [InlineData("x-speakeasy-enums")]
    public void FromOpenApi_WithSupportedNameExtension_UsesCustomNames(string extensionKey)
    {
        var values = new List<IOpenApiAny>
        {
            new OpenApiInteger(0),
            new OpenApiInteger(1)
        };

        var extensions = new Dictionary<string, IOpenApiExtension>
        {
            {
                extensionKey, new OpenApiArray
                {
                    new OpenApiString("Active"),
                    new OpenApiString("Inactive")
                }
            }
        };

        var result = _target.FromOpenApi(values, extensions);

        result.Count.ShouldBe(2);
        result[0].Name.ShouldBe("Active");
        result[0].Value.ShouldBe("0");
        result[1].Name.ShouldBe("Inactive");
        result[1].Value.ShouldBe("1");
    }

    [Fact]
    public void FromOpenApi_NameExtensionWithMismatchedCount_FallsBackToItemN()
    {
        var values = new List<IOpenApiAny>
        {
            new OpenApiString("a"),
            new OpenApiString("b"),
            new OpenApiString("c")
        };

        var extensions = new Dictionary<string, IOpenApiExtension>
        {
            {
                "x-enumNames", new OpenApiArray
                {
                    new OpenApiString("OnlyOne")
                }
            }
        };

        var result = _target.FromOpenApi(values, extensions);

        result.Count.ShouldBe(3);
        result[0].Name.ShouldBe("Item0");
        result[1].Name.ShouldBe("Item1");
    }
}
