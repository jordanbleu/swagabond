using Microsoft.Extensions.Logging;
using Swagabond.Cli.IO;
using Swagabond.Configuration.Instructions;
using Swagabond.Core.ObjectModel;
using Swagabond.Templates;

namespace Swagabond.Cli.Execution;

public class ExecutionPlanBuilder
{
    private readonly TemplateEngineFactory _templateEngineFactory;
    private readonly ILogger _logger;
    private readonly IDataRetriever _dataRetriever;
    
    public ExecutionPlanBuilder(TemplateEngineFactory templateEngineFactory, ILogger logger, IDataRetriever dataRetriever)
    {
        _templateEngineFactory = templateEngineFactory;
        _logger = logger;
        _dataRetriever = dataRetriever;
    }

    /// <summary>
    /// The key is the filename, and the value is each task that will output to that file.
    /// </summary>
    private Dictionary<string, Func<Task>> _executionPlan { get; set; } = new();
    
    /// <summary>
    /// evaluate the template with the passed in model object and write the output to a file.
    /// </summary>
    /// <param name="startingDirectory"></param>
    /// <param name="instruction"></param>
    /// <returns></returns>
    private async Task RenderOutputToFile<T>(string startingDirectory, 
        InstructionSet instructionSet, ProcessTemplateInstruction instruction, T model, string outputFile)
    {
        _logger.LogInformation("Begin rendering template {0} to {1}", instruction.TemplateFile, outputFile);

        var instructionSetBaseTemplateDirectory = instructionSet.TemplateBaseDirectory;
        var instructionTemplateFilePath = instruction.TemplateFile;
        var fullPathToTemplateFile = Path.Combine(startingDirectory, instructionSetBaseTemplateDirectory, instructionTemplateFilePath);
        
        if (!File.Exists(fullPathToTemplateFile))
            throw new FileNotFoundException($"Template file not found at {fullPathToTemplateFile}.");

        _logger.LogInformation("Downloading template {0}", instruction.TemplateFile);

        await using var templateContentStream = await _dataRetriever.GetDataStream(fullPathToTemplateFile);
        var templateContents = await _dataRetriever.ReadStreamAsText(templateContentStream);

        _logger.LogInformation("Rendering template {0} for {1}", instruction.TemplateFile, typeof(T).Name);

        string output;
        try
        {
            var templateEngine = _templateEngineFactory.GetEngine(instruction.TemplateType);
            output = await templateEngine.RenderTemplate(templateContents, model);
        } 
        catch (Exception e)
        {
            _logger.LogError(e, "Error rendering template {0} for {1}", instruction.TemplateFile, typeof(T).Name);
            throw;
        }

        var instructionSetBaseOutputDirectory = instructionSet.OutputBaseDirectory;
        var finalOutputPath = Path.Combine(startingDirectory, instructionSetBaseOutputDirectory, outputFile);
        var finalOutputPathDirectory = Path.GetDirectoryName(finalOutputPath) ?? ".";
        
        // ensure the directory exists
        if (!Directory.Exists(finalOutputPathDirectory))
        {
            _logger.LogInformation("Creating missing dir {}", finalOutputPathDirectory);
            Directory.CreateDirectory(finalOutputPathDirectory);
        }
        
        _logger.LogInformation("Writing template output to {0}", finalOutputPath);
        await File.WriteAllTextAsync(finalOutputPath, output);
    }

    public async Task AddApiScopedInstruction(string startingDirectory, InstructionSet instructionSet, ProcessTemplateInstruction instruction, Api api)
    {
        var templateEngine = _templateEngineFactory.GetEngine(instruction.TemplateType);

        var fileNameTemplate = instruction.OutputFileNameTemplate;
        var outputFileName = await templateEngine.RenderTemplate(fileNameTemplate, api);

        if (_executionPlan.ContainsKey(outputFileName))
            throw new InvalidOperationException($"Output template text '{fileNameTemplate}' would result in overlapping filenames: '{outputFileName}'." +
                                                $"Please double check that each api-scoped instruction writes out a unique filename.");

        _executionPlan.Add(outputFileName,
            () => RenderOutputToFile(startingDirectory, instructionSet, instruction, api, outputFileName));
    }
    
    public async Task AddPathScopedInstruction(string startingDirectory, InstructionSet instructionSet, ProcessTemplateInstruction instruction, ApiPath path)
    {
        var templateEngine = _templateEngineFactory.GetEngine(instruction.TemplateType);

        var fileNameTemplate = instruction.OutputFileNameTemplate;
        var outputFileName = await templateEngine.RenderTemplate(fileNameTemplate, path);

        if (_executionPlan.ContainsKey(outputFileName))
            throw new InvalidOperationException($"Output template text '{fileNameTemplate}' would result in overlapping filenames: '{outputFileName}'." +
                                                $"Please double check that each path-scoped instruction writes out a unique filename.");
        
        _executionPlan.Add(outputFileName,
            () => RenderOutputToFile(startingDirectory, instructionSet, instruction, path, outputFileName));
    }

    public async Task AddOperationScopedInstruction(string startingDirectory, InstructionSet instructionSet,
        ProcessTemplateInstruction instruction, ApiOperation operation)
    {
        var templateEngine = _templateEngineFactory.GetEngine(instruction.TemplateType);

        var fileNameTemplate = instruction.OutputFileNameTemplate;
        var outputFileName = await templateEngine.RenderTemplate(fileNameTemplate, operation);

        if (_executionPlan.ContainsKey(outputFileName))
            throw new InvalidOperationException($"Output template text '{fileNameTemplate}' would result in overlapping filenames: '{outputFileName}'." +
                                                $"Please double check that each opearation scoped instruction writes out a unique filename.");
        
        _executionPlan.Add(outputFileName,
            () => RenderOutputToFile(startingDirectory, instructionSet, instruction, operation, outputFileName));
        
    }
    
    public async Task AddSchemaScopedInstruction(string startingDirectory, InstructionSet instructionSet,
        ProcessTemplateInstruction instruction, ApiSchema schema)
    {
        var templateEngine = _templateEngineFactory.GetEngine(instruction.TemplateType);

        var fileNameTemplate = instruction.OutputFileNameTemplate;
        var outputFileName = await templateEngine.RenderTemplate(fileNameTemplate, schema);

        if (_executionPlan.ContainsKey(outputFileName))
            throw new InvalidOperationException($"Output template text '{fileNameTemplate}' would result in overlapping filenames: '{outputFileName}'." +
                                                $"Please double check that each schema scoped instruction writes out a unique filename.");
        
        _executionPlan.Add(outputFileName,
            () => RenderOutputToFile(startingDirectory, instructionSet, instruction, schema, outputFileName));
        
    }

    public async Task Execute(int maxDegreeOfParallelism)
    {
        if (maxDegreeOfParallelism < 0) 
            maxDegreeOfParallelism = 1;

        var executionPlanTasks = _executionPlan.Values.ToList();
    
        var semaphore = new SemaphoreSlim(maxDegreeOfParallelism);
    
        try
        {
            var taskList = new List<Task>();

            foreach (var task in executionPlanTasks)
            {
                await semaphore.WaitAsync();

                taskList.Add(Task.Run(async () =>
                {
                    try
                    {
                        await task();
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }));
            }

            await Task.WhenAll(taskList);
        }
        finally
        {
            semaphore.Dispose();
        }
    }
    



}