using Shouldly;
using Swagabond.Core.Extensions;

namespace Swagabond.Tests.Core;

public class CoreStringExtensionsTests
{
    [Theory]
    [InlineData("hello world", "HelloWorld")]
    [InlineData("api_version", "ApiVersion")]
    [InlineData("already", "Already")]
    [InlineData("camelCase", "Camelcase")]
    [InlineData("api.v1.endpoint", "ApiV1Endpoint")]
    [InlineData("x-enum-varnames", "XEnumVarnames")]
    public void ToAlphaNumericCamelCase_ConvertsCorrectly(string input, string expected)
    {
        input.ToAlphaNumericCamelCase().ShouldBe(expected);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ToAlphaNumericCamelCase_ReturnsEmptyForNullOrEmpty(string? input)
    {
        input.ToAlphaNumericCamelCase().ShouldBe(string.Empty);
    }

    [Fact]
    public void ToAlphaNumericCamelCase_AllSpecialChars_ReturnsEmpty()
    {
        "---".ToAlphaNumericCamelCase().ShouldBe(string.Empty);
        "!!!".ToAlphaNumericCamelCase().ShouldBe(string.Empty);
        "...".ToAlphaNumericCamelCase().ShouldBe(string.Empty);
    }

    [Fact]
    public void ToAlphaNumericCamelCase_StartsWithDigit_PrefixedWithC()
    {
        var result = "123abc".ToAlphaNumericCamelCase();
        result.ShouldStartWith("C");
        result.ShouldBe("C123abc");
    }

    [Fact]
    public void ToAlphaNumericCamelCase_SingleWord_Capitalised()
    {
        "hello".ToAlphaNumericCamelCase().ShouldBe("Hello");
    }
}
