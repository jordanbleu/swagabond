using Microsoft.Extensions.Logging;

namespace Swagabond.Cli.ConsoleServices;

public class ConsoleHelper
{
    public static bool Verbose = true;
    
    private const string LogoAsciiArt =
@"
█▀     █ █ █     ▄▀█     █▀▀     ▄▀█     █▄▄     █▀█     █▄ █     █▀▄
▄█     ▀▄▀▄▀     █▀█     █▄█     █▀█     █▄█     █▄█     █ ▀█     █▄▀
";
    
    private const string RobotAsciiArt = 
@"       ▄█▄▄▄█▄
▄▀    ▄▌─▄─▄─▐▄    ▀▄
█▄▄█  ▀▌─▀─▀─▐▀  █▄▄█  {0}
 ▐▌    ▀▀███▀▀    ▐▌      {1}
████ ▄█████████▄ ████
";

    private const string FailAsciiArt = 
@"
▄██████████████▄▐█▄▄▄▄█▌
██████▌▄▌▄▐▐▌███▌▀▀██▀▀
████▄█▌▄▌▄▐▐▌▀███▄▄█▌
▄▄▄▄▄██████████████▀
";
    
    private readonly ILogger<Program> _logger;
    
    public ConsoleHelper(ILogger<Program> logger)
    {
        _logger = logger;
    }
    
    private const string border = "-----------------------------------------------------------";
    
    public void WriteBorder()
    {
        System.Console.WriteLine(border);
    }

    public void WriteInnerBorderedText(string text)
    {
        System.Console.WriteLine($"|- {text} ".PadRight(border.Length-3) + " -|");
    }
    
    public void WriteError(string message, IEnumerable<string> details, Exception? ex = null)
    {
        DrawFailWhale();
        _logger.LogError($"{message}\n{string.Join("\n", details)}", ex);
    }

    public void WriteError(string message)
    {
        DrawFailWhale();
        _logger.LogError(message);
    }

    public void DrawBanner()
    {
        if (Verbose)
        {
            System.Console.WriteLine(LogoAsciiArt);
            return;
        }

        System.Console.WriteLine("SWAGaBOND is running...");
    }



    public void DrawRobotAscii(string message, string message2 = "")
    {
        if (!Verbose)
        {
            Console.WriteLine("Swagabond Completed Successfully!");
        }

        _logger.LogInformation(string.Format(RobotAsciiArt, message, message2));
    }
 
    public void DrawFailWhale() => _logger.LogInformation(FailAsciiArt);
}