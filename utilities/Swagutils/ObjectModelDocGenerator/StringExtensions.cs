namespace ObjectModelDocGenerator;

public static class StringExtensions
{
    extension(string input)
    {
        public string RemoveNewlines()
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            // Remove newlines and extra spaces
            return input
                .Replace("\r\n", " ")
                .Replace("\n", " ")
                .Replace("\r", " ")
                .Trim();
        }
    }
}