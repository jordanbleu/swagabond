using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

// ** Disable the 'never null' warnings since input comes from templates **  
// ReSharper disable ConditionalAccessQualifierIsNonNullableAccordingToAPIContract

namespace Swagabond.Templates.Functions;

/// <summary>
/// This contains functions that are callable from templates.
///
/// Note - by Swagabond convention functions are camelCase and prefixed by 'f_'
/// </summary>
public class TemplateFunctions
{
    /// <summary>
    /// Converts the input string to UPPERCASE
    /// </summary>
    /// <param name="input">input</param>
    public static string Upper(string input)
        => input?.ToUpperInvariant() ?? string.Empty;

    /// <summary>
    /// Returns the input string in lowercase
    /// </summary>
    /// <param name="input">input</param>
    public static string Lower(string input)
        => input?.ToLowerInvariant() ?? string.Empty;        
    
    /// <summary>
    /// URL Encodes the input string. 
    /// </summary>
    /// <param name="input">input</param>
    public static string UrlEncode(string input)
        => UrlEncoder.Default.Encode(input);
    
    /// <summary>
    /// If the input value is null or whitespace, returns the default value.
    /// </summary>
    /// <param name="input">input value</param>
    /// <param name="defaultValue">the fallback value if input is null</param>
    /// <returns></returns>
    public static string Coalesce(string input, string defaultValue)
        => string.IsNullOrWhiteSpace(input) ? defaultValue : input;

    /// <summary>
    /// Replaces new lines with spaces (environment agnostic).
    /// </summary>
    /// <param name="input">input string</param>
    public static string StripNewlines(string input)
        => input?.ReplaceLineEndings(" ") ?? string.Empty;

    /// <summary>
    /// Splits your string on any non alpha numeric tokens and combines them using PascalCase notation.
    /// </summary>
    /// <param name="input">input string</param>
    public static string PascalCase(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        var words = input.Split(new[] { '_', '-', ' ', '.' }, StringSplitOptions.RemoveEmptyEntries);

        if (words.Length == 1)
        {
            var word = words[0];
            return char.ToUpperInvariant(word[0]) + word.Substring(1);
        }

        return string.Concat(words.Select(word =>
            char.ToUpperInvariant(word[0]) + word.Substring(1).ToLowerInvariant()));
    }

    /// <summary>
    /// Splits your string on any non alpha numeric tokens and combines them using PascalCase notation.
    /// </summary>
    /// <param name="input">input string</param>
    public static string CamelCase(string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;
        
        var pascalCase = PascalCase(input);
        
        // just lowercase the first letter 
        return char.ToLower(pascalCase[0]) + pascalCase[1..];
    }

    /// <summary>
    /// Given an input string such as this.is.a.test, will return the final token when splitting by the '.' character.
    /// In that case, it would return 'test'.
    /// </summary>
    /// <param name="input">input string with one or more dots</param>
    public static string LastDottedSegment(string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;
        
        var parts = input.Split('.');
        return parts.LastOrDefault() ?? string.Empty;
    }

    /// <summary>
    /// Inserts the prefix value before each newline character, while still preserving the newline characters.
    /// This can be used for situations where you'd like to preserve newlines but need each newline to have something
    /// in front of it, such as for block comments in code, etc.
    ///
    /// A great example is when you are generating .net summary tags, this method can be used to insert a '///' in front
    /// of each newline.
    /// 
    /// </summary>
    /// <param name="input">input string, with newlines</param>
    /// <param name="prefix">what you want to insert in front of each newline</param>
    /// <returns></returns>
    public static string PrefixNewlines(string input, string prefix)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.None);
        var prefixedLines = lines.Select(line => $"{prefix}{line}");
        return string.Join(Environment.NewLine, prefixedLines);
    }

    /// <summary>
    /// returns true if the input string is null or empty
    /// </summary>
    /// <param name="input">input string</param>
    public static bool IsBlank(string input) => 
        string.IsNullOrWhiteSpace(input);
    
    /// <summary>
    /// Returns a random guid each time it is called.
    /// </summary>
    /// <returns></returns>
    public static string RandomGuid() =>
        Guid.NewGuid().ToString();

    /// <summary>
    /// I wrote this as a joke.  Converts your input to 1337.
    /// Not very useful.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string L33t(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        var upper = input.ToUpperInvariant();
        var leet = new Dictionary<char, string>
        {
            ['A'] = "4",
            ['B'] = "8",
            ['C'] = "<",
            ['E'] = "3",
            ['G'] = "6",
            ['H'] = "#",
            ['I'] = "1",
            ['L'] = "1",
            ['O'] = "0",
            ['S'] = "5",
            ['T'] = "7",
            ['Z'] = "2"
        };

        var result = new System.Text.StringBuilder();
        foreach (var c in upper)
        {
            if (leet.TryGetValue(c, out var replacement))
                result.Append(replacement);
            else
                result.Append(c);
        }

        return result.ToString();
    }

    /// <summary>
    /// Returns the current date as a long string.
    /// Example = Saturday, May 31, 2025
    /// </summary>
    /// <param name="utc">if true, will use utc timezone</param>
    /// <returns></returns>
    public static string GetDate(bool utc = true) =>
        utc ? DateTime.UtcNow.ToLongDateString() : DateTime.Now.ToLongDateString();

    /// <summary>
    /// Returns the current time.
    /// Example: 3:16:56 PM 
    /// </summary>
    /// <param name="utc">if true, will use utc timezone</param>
    /// <returns></returns>
    public static string GetTime(bool utc = true) => 
        utc ? DateTime.UtcNow.ToLongTimeString() : DateTime.Now.ToLongTimeString();

    /// <summary>
    /// returns the current date using a custom format string, following .net's conventions. IF an invalid format string
    /// is passed in, a big angry error will be returned instead of your date / time. 
    /// </summary>
    /// <param name="utc">if true, will use utc timezone</param>\
    /// <param name="format">The .net format specifier.  See: https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings</param>
    /// <returns></returns>
    public static string GetFormattedDateTime(bool utc, string format)
    {
        var dt = utc ? DateTime.UtcNow : DateTime.Now;
        try
        {
            return DateTime.Now.ToString(format);
        }
        catch (Exception e)
        {
            return "!!! GetFormattedDateTime - UNABLE TO FORMAT DATETIME USING FORMAT STRING " + format + "!!!";
        }
    }

    
    






}