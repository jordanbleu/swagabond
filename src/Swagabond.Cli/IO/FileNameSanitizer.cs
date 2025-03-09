using System.Text.RegularExpressions;

namespace Swagabond.Cli.IO;

public static class FileNameSanitizer
{
    public static string SanitizeFileName(string fileName)
    {
        // Define a set of invalid characters for filenames
        char[] invalidChars = Path.GetInvalidFileNameChars();
        
        // Create a regex pattern to match invalid characters
        string invalidCharsPattern = $"[{Regex.Escape(new string(invalidChars))}]";
        
        // Replace invalid characters with an underscore
        string sanitizedFileName = Regex.Replace(fileName, invalidCharsPattern, "_");
        
        return sanitizedFileName;
    }
}