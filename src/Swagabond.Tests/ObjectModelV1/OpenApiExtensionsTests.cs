using Microsoft.OpenApi.Any;
using Shouldly;
using Swagabond.ObjectModelV1.Extensions;

namespace Swagabond.Tests.ObjectModelV1;

public class OpenApiExtensionsTests
{
    [Fact]
    public void WriteAsString_OpenApiString_ReturnsValue()
    {
        new OpenApiString("hello").WriteAsString().ShouldBe("hello");
    }

    [Fact]
    public void WriteAsString_OpenApiInteger_ReturnsStringRepresentation()
    {
        new OpenApiInteger(42).WriteAsString().ShouldBe("42");
    }

    [Fact]
    public void WriteAsString_OpenApiLong_ReturnsStringRepresentation()
    {
        new OpenApiLong(9_999_999_999L).WriteAsString().ShouldBe("9999999999");
    }

    [Fact]
    public void WriteAsString_OpenApiFloat_UsesInvariantCulture()
    {
        new OpenApiFloat(3.14f).WriteAsString().ShouldBe("3.14");
    }

    [Fact]
    public void WriteAsString_OpenApiDouble_UsesInvariantCulture()
    {
        new OpenApiDouble(3.14159).WriteAsString().ShouldBe("3.14159");
    }

    [Fact]
    public void WriteAsString_OpenApiBooleanTrue_ReturnsTrue()
    {
        new OpenApiBoolean(true).WriteAsString().ShouldBe("True");
    }

    [Fact]
    public void WriteAsString_OpenApiBooleanFalse_ReturnsFalse()
    {
        new OpenApiBoolean(false).WriteAsString().ShouldBe("False");
    }

    [Fact]
    public void WriteAsString_OpenApiArray_ReturnsCommaJoined()
    {
        var array = new OpenApiArray
        {
            new OpenApiString("a"),
            new OpenApiString("b"),
            new OpenApiString("c")
        };

        array.WriteAsString().ShouldBe("a,b,c");
    }

    [Fact]
    public void WriteAsString_OpenApiObject_ReturnsKeyColonValuePairs()
    {
        var obj = new OpenApiObject
        {
            ["name"] = new OpenApiString("Rover"),
            ["age"] = new OpenApiInteger(3)
        };

        var result = obj.WriteAsString();
        result.ShouldContain("name:Rover");
        result.ShouldContain("age:3");
    }

    [Fact]
    public void WriteAsString_UnknownType_ReturnsEmpty()
    {
        // OpenApiNull is not handled by the switch
        new OpenApiNull().WriteAsString().ShouldBe(string.Empty);
    }

    [Fact]
    public void WriteAsString_EmptyArray_ReturnsEmpty()
    {
        new OpenApiArray().WriteAsString().ShouldBe(string.Empty);
    }
}
