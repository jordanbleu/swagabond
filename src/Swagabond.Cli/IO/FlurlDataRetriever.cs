using Flurl.Http;

namespace Swagabond.Cli.IO;

public class FlurlDataRetriever 
{
    public Task<Stream> GetDataStream(string input)
    {
        if (!input.EndsWith("json", StringComparison.OrdinalIgnoreCase) && !input.EndsWith("yaml", StringComparison.OrdinalIgnoreCase))
            throw new UriFormatException("Url should end in .json or .yaml");

        return input.GetStreamAsync(HttpCompletionOption.ResponseContentRead);
    }
}