using CommandLine;

namespace Swagabond.Cli.ConsoleServices;

public static class CommandLineParserErrorExtensions
{
    extension(Error error)
    {
        public string ToFriendlyString() =>
            error switch
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