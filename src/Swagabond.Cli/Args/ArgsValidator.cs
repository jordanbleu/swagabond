namespace Swagabond.Cli.Args;

public class ArgsValidator
{
    public static IEnumerable<string> ValidateArgs(Arguments args)
    {
        if (string.IsNullOrEmpty((args.SwaggerFilePath)))
        {
            yield return "Swagger file path is required." ;
        }
        
        if (string.IsNullOrEmpty(args.InstructionSetFilePath))
        {
            yield return "Instruction set file path is required";
        }
        
    }
}