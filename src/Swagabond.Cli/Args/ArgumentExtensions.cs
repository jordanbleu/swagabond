using System.Text;
using CommandLine;
using Swagabond.Cli.ConsoleServices;

namespace Swagabond.Cli.Args;

public static class ArgumentExtensions
{
    public static string WriteErrorString(this IEnumerable<Error> cliErrors)
    {
        var sb = new StringBuilder();

        foreach (var err in cliErrors)
        {
            if (err is TokenError te)
            {
                sb.AppendLine(te.Token + " is not a valid token");
            }
            else
            {
                sb.AppendLine(err.GetType().Name + ": " + err.ToFriendlyString());
            }
        }

        return sb.ToString();
    }

}