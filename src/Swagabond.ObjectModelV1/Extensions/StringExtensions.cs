using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Swagabond.ObjectModelV1.Extensions;

public static class StringExtensions
{
    
    public static string StripNewLines(this string str)
    {
        if (string.IsNullOrWhiteSpace(str))
            return str;

        return str.Replace("\r\n", " ")
            .Replace("\n", " ")
            .Replace("\r", " ");
    }

    
    public static string ToPascalCase(this string input)
    {
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
    
    public static string ToClassName(this string str)
    {
        var name = str.ToPascalCase();
        
        // Remove any non-alphanumeric characters
        name = new string(name.Where(char.IsLetterOrDigit).ToArray());
        
        // if name starts with a number, start with 'N'
        if (char.IsDigit(name[0]))
        {
            name = "N" + name;
        }

        return name;
    }
}