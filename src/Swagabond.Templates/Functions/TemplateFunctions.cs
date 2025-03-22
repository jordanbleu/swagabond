using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

namespace Swagabond.Templates.Functions;

/// <summary>
/// This contains functions that are callable from templates.
///
/// Note - by Swagabond convention functions are camelCase and prefixed by 'f'
/// </summary>
public class TemplateFunctions
{
    public static string Upper(string input)
        => input.ToUpperInvariant();

    public static string Lower(string input)
        => input.ToLowerInvariant();        
    
    public static string UrlEncode(string input)
        => UrlEncoder.Default.Encode(input);
    
    public static string Coalesce(string input, string defaultValue)
        => string.IsNullOrWhiteSpace(input) ? defaultValue : input;

    public static string StripNewlines(string input)
        => input.ReplaceLineEndings(" ");

    /// <summary>
    /// Splits your string on any non alpha numeric tokens and combines them using PascalCase notation.
    /// </summary>
    public static string PascalCase(string str)
    {
        if (string.IsNullOrWhiteSpace(str))
            return str;

        var words = str.Split(new[] { '_', '-', ' ', '.' }, StringSplitOptions.RemoveEmptyEntries);

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
    public static string CamelCase(string str)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;
        
        var pascalCase = PascalCase(str);
        
        // just lowercase the first letter 
        return char.ToLower(pascalCase[0]) + pascalCase[1..];
    }
    
    public static string LastDottedSegment(string str)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;
        
        var parts = str.Split('.');
        return parts.LastOrDefault() ?? string.Empty;
        
    }
}