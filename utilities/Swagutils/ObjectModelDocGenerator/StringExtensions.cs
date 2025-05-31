namespace ObjectModelDocGenerator;

public static class StringExtensions
{
    public static string RemoveNewlines(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        // Remove newlines and extra spaces
        return input.Replace("\n", " ").Replace("\r", " ").Trim();
    }
    
}