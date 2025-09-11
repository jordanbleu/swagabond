using CommandLine;

namespace Swagabond.Cli.Args;

public class Arguments
{
    [Option('s', "swagger", HelpText = "File path or URL to the swagger file (json or yaml)", Required = true)]
    public string SwaggerFilePath { get; set; } = string.Empty;

    [Option('o', "output", HelpText = "Output directory for generated files", Required = false)]
    public string OutputDirectory { get; set; } = "./output";

    [Option('i', "instructions", HelpText = "File path or URL to your instruction set (yaml)", Required = true)]
    public string InstructionSetFilePath { get; set; } = string.Empty;

    [Option('e', "failOnApiSpecError", HelpText = "If true, client generation will be halted if any errors exist with your API spec.", Required = false)]
    public string FailOnApiSpecError { get; set; } = "true";
    
    [Option('w', "failOnApiSpecWarning", HelpText = "If true, client generation will be halted if any warnings exist with your API spec.", Required = false)]
    public string FailOnApiSpecWarning{ get; set; } = "true";

    [Option('p', 
        "maxDop",
        HelpText = "Max Degree of Parallelism.  Basically, this controls how many files you can generate at once.  Lower value means slower but use less memory / CPU threads.  Higher value means faster and use more memory / CPU threads.",
        Required = false)]
    public int MaxDegreeOfParallelism { get; set; } = 5;
    
    [Option(shortName: 'c', longName: "cleanOutput", HelpText = "If true, the output directory will be cleaned before generating new files.", Required = false)]
    public string CleanOutputDirectory { get; set; } = "false";

    [Option(shortName: 'j', longName: "dumpJson", HelpText = "If true, a json file will be generated and saved to the output dir.", Required = false)]
    public string DumpJson { get; set; } = "false";

    [Option(shortName: 'v', longName: "verbose", HelpText = "If true, the console output will be much more informative and verbose.", Required = false)]
    public string Verbose { get; set; } = "false";
}