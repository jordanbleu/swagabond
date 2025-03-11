using Microsoft.Extensions.Logging;

namespace Swagabond.Cli.ConsoleServices;

public class ConsoleHelper
{
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
    
    public void WriteError(string message, IEnumerable<string> details, Exception ex = null)
    {
        DrawFailWhale();
        _logger.LogError($"{message}\n{string.Join("\n", details)}", ex);
    }

    public void WriteError(string message)
    {
        DrawFailWhale();
        _logger.LogInformation(message);
    }

    public void DrawBanner() =>System.Console.WriteLine(LogoAsciiArt);

    public void DrawRobotAscii(string message, string message2 = "")
    {
        _logger.LogInformation(string.Format(RobotAsciiArt, message, message2));
    }
 
    public void DrawFailWhale() => _logger.LogInformation(FailAsciiArt);
}