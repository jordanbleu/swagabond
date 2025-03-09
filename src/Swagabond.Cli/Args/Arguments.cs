using CommandLine;

namespace Swagabond.Cli.Args;

public class Arguments
{
    [Option('s', "swagger", HelpText = "File path or URL to the swagger file (json or yaml)", Required = true)]
    public string SwaggerFilePath { get; set; }

    [Option('i', "instructions", HelpText = "File path or URL to your instruction set (yaml)", Required = true)]
    public string InstructionSetFilePath { get; set; }

    [Option('e', "failOnApiSpecError", HelpText = "If true, client generation will be halted if any errors exist with your API spec.", Required = false)]
    public bool FailOnApiSpecError { get; set; } = true;
    
    [Option('w', "failOnApiSpecWarning", HelpText = "If true, client generation will be halted if any warnings exist with your API spec.", Required = false)]
    public bool FailOnApiSpecWarning{ get; set; } = true;

    [Option('p', 
        "maxDop",
        HelpText = "Max Degree of Parallelism.  Basically, this controls how many files you can generate at once.  Lower values are slower but use less memory / CPU.  Higher values are faster and use more memory / CPU.",
        Required = false)]
    public int MaxDegreeOfParallelism { get; set; } = 5;
}