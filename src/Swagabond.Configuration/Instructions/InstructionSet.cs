using Swagabond.Templates;
using YamlDotNet.Serialization;

namespace Swagabond.Configuration.Instructions;

/// <summary>
/// Used to tell Swagabond what to do
/// </summary>
public class InstructionSet
{
    /// <summary>
    /// Where the base directory should be for templates, relative to where
    /// the instruction set is
    /// </summary>
    [YamlMember(Alias = "template_base_directory")]
    public string TemplateBaseDirectory { get; set; } = ".";

    /// <summary>
    /// Where the base output directory should be for outputs that have been processed,
    /// relative to where the instruction set is
    /// </summary>
    [YamlMember(Alias = "output_base_directory")]
    public string OutputBaseDirectory { get; set; } = ".";
    
    [YamlMember(Alias = "for_api")]
    public List<ProcessTemplateInstruction> ApiScopedInstructions { get; set; } = new();

    [YamlMember(Alias = "for_schema_definitions")]
    public List<ProcessTemplateInstruction> SchemaScopedInstructions { get; set; } = new();
    
    [YamlMember(Alias = "for_operations")]
    public List<ProcessTemplateInstruction> OperationScopedInstructions { get; set; } = new();

    [YamlMember(Alias = "for_paths")]
    public List<ProcessTemplateInstruction> PathScopedInstructions { get; set; } = new();
    
    /// <summary>
    /// Any arbitrary metadata that can be accessed via templates
    /// </summary>
    [YamlMember(Alias = "metadata")]
    public Dictionary<string, string> Metadata { get; set; } = new();
}

public class ProcessTemplateInstruction
{
    [YamlMember(Alias = "use_template_file")]
    public string TemplateFile { get; set; }

    [YamlMember(Alias = "write_output_to")]
    public string OutputFileNameTemplate { get; set; }

    [YamlMember(Alias = "use_template_processor")]
    public TemplateType TemplateType { get; set; } = TemplateType.Scriban;
}
