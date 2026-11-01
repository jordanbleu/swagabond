using System.Text.RegularExpressions;

namespace Swagabond.ObjectModelV1.Extensions;

public static partial class StringExtensions
{

    extension(string str)
    {
        public string StripNewLines()
        {
            if (string.IsNullOrWhiteSpace(str))
                return str;

            return str.Replace("\r\n", " ")
                .Replace("\n", " ")
                .Replace("\r", " ");
        }

        public string ToClassName()
        {
            var name = str.ToPascalCase();

            // Remove any non-alphanumeric characters
            name = new string([.. name.Where(char.IsLetterOrDigit)]);

            if (name.Length == 0)
                return name;

            // if name starts with a number, start with 'N'
            if (char.IsDigit(name[0]))
            {
                name = "N" + name;
            }

            return name;
        }
    }

    [GeneratedRegex("[^_a-zA-Z0-9]", RegexOptions.Compiled)]
    private static partial Regex InvalidCharsRgx();

    [GeneratedRegex(@"(?<=\s)", RegexOptions.Compiled)]
    private static partial Regex WhiteSpace();

    [GeneratedRegex("^[a-z]", RegexOptions.Compiled)]
    private static partial Regex StartsWithLowerCaseChar();

    [GeneratedRegex("(?<=[A-Z])[A-Z0-9]+$", RegexOptions.Compiled)]
    private static partial Regex FirstCharFollowedByUpperCasesOnly();

    [GeneratedRegex("(?<=[0-9])[a-z]", RegexOptions.Compiled)]
    private static partial Regex LowerCaseNextToNumber();

    [GeneratedRegex("(?<=[A-Z])[A-Z]+?((?=[A-Z][a-z])|(?=[0-9]))", RegexOptions.Compiled)]
    private static partial Regex UpperCaseInside();

    public static string ToPascalCase(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        // replace white spaces with undescore, then replace all invalid chars with empty string
        var pascalCase = InvalidCharsRgx().Replace(WhiteSpace().Replace(input, "_"), string.Empty)
            // split by underscores
            .Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
            // set first letter to uppercase
            .Select(w => StartsWithLowerCaseChar().Replace(w, m => m.Value.ToUpper()))
            // replace second and all following upper case letters to lower if there is no next lower (ABC -> Abc)
            .Select(w => FirstCharFollowedByUpperCasesOnly().Replace(w, m => m.Value.ToLower()))
            // set upper case the first lower case following a number (Ab9cd -> Ab9Cd)
            .Select(w => LowerCaseNextToNumber().Replace(w, m => m.Value.ToUpper()))
            // lower second and next upper case letters except the last if it follows by any lower (ABcDEf -> AbcDef)
            .Select(w => UpperCaseInside().Replace(w, m => m.Value.ToLower()));

        var result = string.Concat(pascalCase);


        return result;
    }
}