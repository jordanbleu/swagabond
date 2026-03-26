using Shouldly;
using Swagabond.ObjectModelV1.Extensions;

namespace Swagabond.Tests.ObjectModelV1;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("hello world", "HelloWorld")]
    [InlineData("some_snake_case", "SomeSnakeCase")]
    [InlineData("already PascalCase", "AlreadyPascalCase")]
    [InlineData("with_underscores", "WithUnderscores")]
    [InlineData("ALL_CAPS", "AllCaps")]
    [InlineData("camelCase", "CamelCase")]
    public void ToPascalCase_ConvertsCorrectly(string input, string expected)
    {
        input.ToPascalCase().ShouldBe(expected);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ToPascalCase_ReturnsEmptyForBlankInput(string? input)
    {
        input.ToPascalCase().ShouldBe(string.Empty);
    }

    [Theory]
    [InlineData("Pet", "Pet")]
    [InlineData("my_schema", "MySchema")]
    [InlineData("G4.Marketing.CustomPixelValues", "G4MarketingCustomPixelValues")]
    public void ToClassName_ProducesValidIdentifier(string input, string expected)
    {
        input.ToClassName().ShouldBe(expected);
    }

    [Fact]
    public void ToClassName_LeadingDigit_PrependedWithN()
    {
        "123Schema".ToClassName().ShouldStartWith("N");
    }

    [Theory]
    [InlineData("line1\nline2", "line1 line2")]
    [InlineData("line1\r\nline2", "line1 line2")]
    [InlineData("line1\rline2", "line1 line2")]
    [InlineData("no newlines", "no newlines")]
    public void StripNewLines_ReplacesAllNewlineStyles(string input, string expected)
    {
        input.StripNewLines().ShouldBe(expected);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void StripNewLines_ReturnsInputForBlankStrings(string? input)
    {
        input.StripNewLines().ShouldBe(input);
    }
}
