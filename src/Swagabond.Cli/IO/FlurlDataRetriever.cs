using Flurl.Http;

namespace Swagabond.Cli.IO;

public class FlurlDataRetriever 
{
    public Task<Stream> GetDataStream(string input) =>
        input.GetStreamAsync(HttpCompletionOption.ResponseContentRead);
}