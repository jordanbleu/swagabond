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

    public static string ToPascalCase(this string str)
    {
        if (string.IsNullOrWhiteSpace(str))
            return str;

        str = str.StripNewLines();

        var words = str.Split(new[] { '_', '-', ' ', '.', '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);

        if (words.Length == 1)
        {
            var word = words[0];
            return char.ToUpperInvariant(word[0]) + word.Substring(1);
        }

        return string.Concat(words.Select(word =>
            char.ToUpperInvariant(word[0]) + word.Substring(1).ToLowerInvariant()));
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