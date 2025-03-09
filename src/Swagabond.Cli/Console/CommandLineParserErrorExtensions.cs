using CommandLine;

namespace Swagabond.Cli.Console;

public static class CommandLineParserErrorExtensions
{
    public static string ToFriendlyString(this Error error)
    {
        return error switch
        {
            MissingRequiredOptionError missingRequiredOptionError => 
                $"Error: Missing required option '{missingRequiredOptionError.NameInfo.NameText}'.",
            UnknownOptionError => "Error: Unknown option.",
            BadFormatConversionError => "Error: Bad format conversion.",
            SetValueExceptionError => "Error: Set value exception.",
            SequenceOutOfRangeError => "Error: Sequence out of range.",
            _ => $"Error: {error.Tag}"
        };
    }
    
}