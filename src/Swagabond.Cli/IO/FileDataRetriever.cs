namespace Swagabond.Cli.IO;

public class FileDataRetriever
{
    public Task<Stream> GetDataStream(string input)
    {
        if (!File.Exists(input))
        {
            throw new FileNotFoundException("File not found", input);
        }

        var fs = new FileStream(input, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, bufferSize: 4096,
            useAsync: true);

        return Task.FromResult((Stream)fs);
    }
}