using System.Reflection;
using Microsoft.Extensions.Logging;
using Swagabond.Cli;
using Swagabond.Cli.Args;
using Swagabond.Cli.Console;
using Swagabond.Cli.Execution;
using Swagabond.Cli.IO;
using Swagabond.Configuration.Instructions;
using Swagabond.Core;
using Swagabond.Core.Mappers;
using Swagabond.Templates;
using Swagabond.Templates.Engines;
using YamlDotNet.Core;

// ************************************************
// Map dependencies.  Avoid using DI for now (laziness)
// ************************************************
var dataRetriever = new DataRetriever();

ILogger logger;
using (var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole()))
{
    logger = loggerFactory.CreateLogger("Swagabond.Cli");
}

var consoleHelper = new ConsoleHelper(logger);

var mapper = new OpenApiMapper(logger);

var templateEngineFactory = new TemplateEngineFactory(new[] 
    {
        new ScribanTemplateEngine(),
    });

// ************************************************
// Draw the banner / intro 
// ************************************************
var version = Assembly.GetExecutingAssembly().GetName().Version;
consoleHelper.DrawBanner();
consoleHelper.WriteBorder();
consoleHelper.WriteInnerBorderedText($"OpenAPI Template Processor v{version}");
consoleHelper.WriteInnerBorderedText($"Original Concept by Jordan Bleu");
consoleHelper.WriteInnerBorderedText($"Ascii Art Stolen from https://fsymbols.com/text-art");
consoleHelper.WriteBorder();
Console.WriteLine();


// parse input args
var parser = new CommandLine.Parser();
var argumentsResult = parser.ParseArguments<Arguments>(args);

if (argumentsResult.Errors.Any())
{
    consoleHelper.WriteError("CLI Arguments are not valid >:( ", argumentsResult.Errors.Select(e=>e.ToFriendlyString()));
    return;
}

var arguments = argumentsResult.Value;

// ************************************************
// Tell the user what the input args are
// ************************************************
logger.LogInformation("Input Args:");
logger.LogInformation($"SwaggerFile: {arguments.SwaggerFilePath}");
logger.LogInformation($"InstructionSet: {arguments.InstructionSetFilePath}");


// ************************************************
// Parse instruction set
// ************************************************
logger.LogInformation("Reading and parsing your instructions (the yaml file)...");
InstructionSet instructionSet;
try
{
    var yamlStream = await dataRetriever.GetDataStream(arguments.InstructionSetFilePath);
    var yamlText = await dataRetriever.ReadStreamAsText(yamlStream);

    if (string.IsNullOrEmpty(yamlText))
        throw new InvalidDataException("InstructionSet doesn't exist or was empty");

    instructionSet = InstructionSetSerializer.Deserialize(yamlText);
}
catch (YamlException ex)
{
    consoleHelper.WriteError("An error occurred while parsing the InstructionSet file", new[] { ex.Message }, ex);
    return;
}
catch (FileNotFoundException ex)
{
    consoleHelper.WriteError($"InstructionSet file not found at {arguments.InstructionSetFilePath}",
        Enumerable.Empty<string>(), ex);
    return;
}
catch (Exception ex)
{
    consoleHelper.WriteError("An error occurred while reading/parsing the InstructionSet file", new[] { ex.Message },
        ex);
    return;
}

// ************************************************
// Create a stream that points to the swagger file.
// ************************************************
logger.LogInformation("Creating stream to your swagger file...");
var swaggerStream = await dataRetriever.GetDataStream(arguments.SwaggerFilePath);

// ************************************************
// Map to our object model
// ************************************************

// Build a request for the core logic
logger.LogInformation("Retrieving / parsing / mapping swagger file...");

var request = new MapperRequest()
{
    FailOnDefinitionError = arguments.FailOnApiSpecError,
    FailOnDefinitionWarning = arguments.FailOnApiSpecWarning,
    Metadata = instructionSet.Metadata
};

var api = await mapper.MapFromStream(request, swaggerStream);

// ************************************************
// Build an execution plan for generating this stuff
// ************************************************
logger.LogInformation("Building an execution plan...");

var executionPlan = new ExecutionPlanBuilder(templateEngineFactory, logger, dataRetriever);

var instructionSetBaseDir = Path.GetDirectoryName(arguments.InstructionSetFilePath) ?? ".";

try
{
    foreach (var apiInst in instructionSet.ApiScopedInstructions)
    {
        await executionPlan.AddApiScopedInstruction(instructionSetBaseDir, instructionSet, apiInst, api);
    }

    foreach (var pathInst in instructionSet.PathScopedInstructions)
    {
        foreach (var path in api.Paths)
        {
            await executionPlan.AddPathScopedInstruction(instructionSetBaseDir, instructionSet, pathInst, path);
        }
    }
    
    foreach (var operationInst in instructionSet.OperationScopedInstructions)
    {
        foreach (var path in api.Paths)
        {
            foreach (var operation in path.Operations)
            {
                await executionPlan.AddOperationScopedInstruction(instructionSetBaseDir, instructionSet, operationInst, operation);
            }
        }
    }
}
catch (Exception ex)
{
    consoleHelper.WriteError("There was an error building an execution plan :(", new[]{ex.Message}, ex);
    throw;
}

// ************************************************
// Execute the plan
// ************************************************
logger.LogInformation("Begin generating output...");
try
{
    await executionPlan.Execute(arguments.MaxDegreeOfParallelism);
}
catch (Exception ex)
{
    consoleHelper.WriteError("There was an error (or a bunch maybe) generating output.", new[]{ex.Message}, ex);
    throw;
}


logger.LogInformation("\n\n");
consoleHelper.DrawRobotAscii("Done, please enjoy your output.", "Beep boop boop beep bop.");