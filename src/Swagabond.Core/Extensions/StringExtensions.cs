using System.Text.RegularExpressions;

namespace Swagabond.Core.Extensions;

public static class StringExtensions
{
    public static string ToAlphaNumericCamelCase(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;
        
        // split by any character that is not alphanumeric
        var words = Regex.Split(str, @"[^a-zA-Z0-9]+");
        
        // convert each word into title case
        for (var i = 0; i < words.Length; i++)
        {
            if (string.IsNullOrEmpty(words[i]))
                continue;
            
            var word = words[i];
            words[i] = char.ToUpper(word[0]) + word[1..].ToLower();
        }
        
        // join the words back together
        var joined = string.Join(string.Empty, words);
        
        // ensure the first letter is a character
        if (char.IsDigit(joined[0]))
            joined = "C" + joined;

        return joined;
    }
}