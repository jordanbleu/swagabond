using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;

// ReSharper disable MemberCanBePrivate.Global

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
    private const string QuotationMark = "\"";
    private const string LeftBraces = "{{";
    private const string RightBraces = "}}";
    
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
    public static string UrlEncode(string? input)
        => Uri.EscapeDataString(input ?? string.Empty);

    /// <summary>
    /// If the input value is null or whitespace, returns the default value.
    /// </summary>
    /// <param name="input">input value</param>
    /// <param name="defaultValue">the fallback value if input is null</param>
    /// <returns></returns>
    public static string Coalesce(string? input, string defaultValue)
        => string.IsNullOrWhiteSpace(input) ? defaultValue : input;

    /// <summary>
    /// Replaces new lines with spaces (environment agnostic).
    /// </summary>
    /// <param name="input">input string</param>
    public static string StripNewlines(string input)
        => input?.ReplaceLineEndings(" ") ?? string.Empty;

    /// <summary>
    /// This takes an input as json, and returns it escaped and all on one line.
    /// Quotes will be escaped, " will become \".
    /// Special Characters will be escaped per json rules.
    /// Newlines will be stripped.
    /// Input will be automatically wrapped in quotes.
    /// So for example, input `{ "hello": "world" }` becomes `"{ \"hello\": \"world\" }"` (including outer quotes)
    /// </summary>
    /// <param name="input">Your JSON formatted input</param>
    /// <returns></returns>
    public static string JsonString(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        var serializedAsJson = JsonSerializer.Serialize(input, new JsonSerializerOptions()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });

        return serializedAsJson;
    }

    /// <summary>
    /// Returns a substring of your input string. 
    /// </summary>
    /// <param name="input">input string</param>
    /// <param name="start">which character to begin output</param>
    /// <param name="length">how many characters after start to include</param>
    /// <returns></returns>
    public static string Substring(string input, int start, int length)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;
        
        return input.Substring(start, length);
    }

    /// <summary>
    /// Splits your string on any non alpha numeric tokens and combines them using PascalCase notation.
    /// </summary>
    /// <param name="input">input string</param>
    public static string PascalCase(string input)
    {
        // Copied from https://stackoverflow.com/questions/18627112/how-can-i-convert-text-to-pascal-case 
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;
        
        var invalidCharsRgx = new Regex("[^_a-zA-Z0-9]");
        var whiteSpace = new Regex(@"(?<=\s)");
        var startsWithLowerCaseChar = new Regex("^[a-z]");
        var firstCharFollowedByUpperCasesOnly = new Regex("(?<=[A-Z])[A-Z0-9]+$");
        var lowerCaseNextToNumber = new Regex("(?<=[0-9])[a-z]");
        var upperCaseInside = new Regex("(?<=[A-Z])[A-Z]+?((?=[A-Z][a-z])|(?=[0-9]))");

        // replace white spaces with undescore, then replace all invalid chars with empty string
        var pascalCase = invalidCharsRgx.Replace(whiteSpace.Replace(input, "_"), string.Empty)
            // split by underscores
            .Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
            // set first letter to uppercase
            .Select(w => startsWithLowerCaseChar.Replace(w, m => m.Value.ToUpper()))
            // replace second and all following upper case letters to lower if there is no next lower (ABC -> Abc)
            .Select(w => firstCharFollowedByUpperCasesOnly.Replace(w, m => m.Value.ToLower()))
            // set upper case the first lower case following a number (Ab9cd -> Ab9Cd)
            .Select(w => lowerCaseNextToNumber.Replace(w, m => m.Value.ToUpper()))
            // lower second and next upper case letters except the last if it follows by any lower (ABcDEf -> AbcDef)
            .Select(w => upperCaseInside.Replace(w, m => m.Value.ToLower()));

        var result = string.Concat(pascalCase);
        
        
        return result;
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

        var lines = input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        var prefixedLines = lines.Skip(1).Select(line => $"{prefix}{line}");
        
        var combinedLines = new List<string> { lines[0] }; // Keep the first line as is
        combinedLines.AddRange(prefixedLines);// Add the prefixed lines
        
        return string.Join(Environment.NewLine, combinedLines);
    }

    /// <summary>
    /// returns true if the input string is null or whitespace
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
    /// Converts your input to 1337.
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

    /// <summary>
    /// Given a list of inputs, will return the first item in the collection.
    /// If the list is empty will return an empty string.
    /// </summary>
    /// <param name="inputs">A collection of inputs</param>
    /// <returns></returns>
    public static string FirstItem(IEnumerable<string> inputs) => 
        inputs?.FirstOrDefault() ?? string.Empty;
    
    /// <summary>
    /// Given a list of inputs, will return the last item in the collection.
    /// If the list is empty will return an empty string.
    /// </summary>
    /// <param name="inputs">A collection of inputs</param>
    /// <returns></returns>
    public static string LastItem(IEnumerable<string> inputs) => 
        inputs?.LastOrDefault() ?? string.Empty;

    /// <summary>
    /// Wraps the input with double quotes.  inputString because "inputString".
    /// </summary>
    /// <param name="input">input string</param>
    /// <returns></returns>
    public static string WrapQuotes(string input) =>
        QuotationMark + input + QuotationMark;

    /// <summary>
    /// Shorthand for the WrapQuotes function
    /// </summary>
    /// <param name="input">input string</param>
    /// <returns></returns>
    public static string Wq(string input) => WrapQuotes(input);
    
    /// <summary>
    /// Wraps the input in double braces without evaluating it's input.
    /// inputString becomes {{inputString}}. Can help avoid awkward template
    /// syntax when you want to literally output double braces..
    /// </summary>
    /// <param name="input">input to wrap</param>
    public static string WrapDoubleBraces(string input) =>
        LeftBraces + input + RightBraces;

    /// <summary>
    /// Shorthand for the WrapBraces function.
    /// </summary>
    /// <param name="input">input string</param>
    public static string Wdb(string input) => WrapDoubleBraces(input);
    
}