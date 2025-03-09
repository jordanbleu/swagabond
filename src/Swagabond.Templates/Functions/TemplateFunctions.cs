using System.Text.Encodings.Web;

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
}