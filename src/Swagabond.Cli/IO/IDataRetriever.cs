namespace Swagabond.Cli.IO;

public interface IDataRetriever
{
    public Task<Stream> GetDataStream(string input);
    public Task<string> ReadStreamAsText(Stream stream);
}

public class DataRetriever : IDataRetriever
{
    private FileDataRetriever _fileDataRetriever =new();
    private FlurlDataRetriever _httpDataRetriever = new();

    public Task<Stream> GetDataStream(string input)
    {
        if (input.StartsWith("http", StringComparison.OrdinalIgnoreCase) ||
            input.StartsWith("https", StringComparison.OrdinalIgnoreCase))
        {
            return _httpDataRetriever.GetDataStream(input);
        }

        return _fileDataRetriever.GetDataStream(input);
    }

    public async Task<string> ReadStreamAsText(Stream stream)
    {
        using var reader = new StreamReader(stream);
        var output = await reader.ReadToEndAsync();
        return output;
    }
}