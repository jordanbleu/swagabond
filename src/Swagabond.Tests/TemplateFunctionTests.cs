using Swagabond.Templates.Functions;

namespace Swagabond.Tests;

public class TemplateFunctionTests
{
    [Fact]
    public void TemplateFunctionUpper_ReturnsUppercase()
    {
        var input = "hello world";
        var result = TemplateFunctions.Upper(input);
        Assert.Equal("HELLO WORLD", result);
    }
    
    [Fact]
    public void TemplateFunctionLower_ReturnsLowercase()
    {
        var input = "HELLO WORLD";
        var result = TemplateFunctions.Lower(input);
        Assert.Equal("hello world", result);
    }
    
    [Fact]
    public void TemplateFunctionUrlEncode_ReturnsEncodedString()
    {
        var input = "hello world!";
        var result = TemplateFunctions.UrlEncode(input);
        Assert.Equal("hello%20world%21", result);
    }
    
    [Fact]
    public void TemplateFunctionCoalesce_ReturnsDefaultValueWhenInputIsNull()
    {
        var defaultValue = "default";
        var result = TemplateFunctions.Coalesce(null, defaultValue);
        Assert.Equal(defaultValue, result);
    }
    
    [Fact]
    public void TemplateFunctionCoalesce_ReturnsInputWhenNotNull()
    {
        var input = "value";
        var defaultValue = "default";
        var result = TemplateFunctions.Coalesce(input, defaultValue);
        Assert.Equal(input, result);
    }
    
    [Fact]
    public void TemplateFunctionStripNewlines_ReplacesNewlinesWithSpaces()
    {
        var input = "Hello\nWorld\r\n!";
        var result = TemplateFunctions.StripNewlines(input);
        Assert.Equal("Hello World !", result);
    }
    
    [Fact]
    public void TemplateFunctionPascalCase_ReturnsPascalCasedString()
    {
        var input = "hello world";
        var result = TemplateFunctions.PascalCase(input);
        Assert.Equal("HelloWorld", result);
    }
    
    [Fact]
    public void TemplateFunctionCamelCase_ReturnsCamelCasedString()
    {
        var input = "hello world";
        var result = TemplateFunctions.CamelCase(input);
        Assert.Equal("helloWorld", result);
    }

    [Fact]
    public void TemplateFunctionLastDottedSegment_ReturnsLastDottedSegment()
    {
        var input = "This.Is.A.Test";
        var result = TemplateFunctions.LastDottedSegment(input);
        Assert.Equal("Test", result);
    }
    
    [Fact]
    public void TemplateFunctionLastDottedSegment_ReturnsEmptyStringForNullInput()
    {
        string input = null;
        var result = TemplateFunctions.LastDottedSegment(input);
        Assert.Equal(string.Empty, result);
    }
    
    [Fact]
    public void TemplateFunctionPrefixNewLines_ReturnsStringWithNewLinesPrefixed_WithUnixNewlines()
    {
        var newLine = Environment.NewLine;
        var input = "Hello\nWorld";
        var result = TemplateFunctions.PrefixNewlines(input, ">>>");
        Assert.Equal($"Hello{newLine}>>>World", result);
    }
    
    [Fact]
    public void TemplateFunctionPrefixNewLines_ReturnsStringWithNewLinesPrefixed_WithWindowsNewlines()
    {
        var newLine = Environment.NewLine;
        var input = "Hello\r\nWorld";
        var result = TemplateFunctions.PrefixNewlines(input, ">>>");
        Assert.Equal($"Hello{newLine}>>>World", result);
    }
    
    [Fact]
    public void TemplateFunctionPrefixNewLines_ReturnsStringWithNewLinesPrefixed_WithOldMacOSNewlines()
    {
        var newLine = Environment.NewLine;
        var input = "Hello\rWorld";
        var result = TemplateFunctions.PrefixNewlines(input, ">>>");
        Assert.Equal($"Hello{newLine}>>>World", result);
    }
    
    [Fact]
    public void TemplateFunctionIsBlank_ReturnsTrueForNullOrEmpty()
    {
        string input = null;
        Assert.True(TemplateFunctions.IsBlank(input));
        
        input = string.Empty;
        Assert.True(TemplateFunctions.IsBlank(input));
        
        input = "   ";
        Assert.True(TemplateFunctions.IsBlank(input));
    }
    
    [Fact]
    public void TemplateFunctionsL33t_ReturnsL33tCasedString()
    {
        var input = "leet speak";
        var result = TemplateFunctions.L33t(input);
        Assert.Equal("1337 5P34K", result);
    }
}