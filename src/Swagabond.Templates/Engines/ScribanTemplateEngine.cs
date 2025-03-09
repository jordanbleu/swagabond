using System.Reflection;
using Scriban;
using Scriban.Runtime;
using Swagabond.Templates.Functions;

namespace Swagabond.Templates.Engines;

public class ScribanTemplateEngine : ITemplateEngine
{
    public async Task<string> RenderTemplate<T>(string templateContent, T model)
    {
        var template = Template.Parse(templateContent);
        
        var context = new TemplateContext();
        context.MemberRenamer = member => member.Name; 

        var functions = new ScriptObject();
        
        
        var globals = new ScriptObject();
        // Import each property of the model as a root level property (so you don't need to prefix the name)
        globals.Import(model, renamer:m=>m.Name);
        // Import all functions the same way but prefix with a lowercase f_
        globals.Import(typeof(TemplateFunctions), renamer:  m=> "f_" + m.Name);
        context.PushGlobal(globals);
    
        return await template.RenderAsync(context);
    }
    

    public bool CanProcess(TemplateType templateType)
    {
        if (templateType is TemplateType.Scriban or TemplateType.Liquid)
        {
            return true;
        }

        return false;   
    }
}