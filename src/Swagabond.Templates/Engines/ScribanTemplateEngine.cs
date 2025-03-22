using System.Reflection;
using System.Text;
using Scriban;
using Scriban.Parsing;
using Scriban.Runtime;
using Swagabond.Templates.Functions;

namespace Swagabond.Templates.Engines;

public class ScribanTemplateEngine : ITemplateEngine
{
    public async Task<string> RenderTemplate<T>(string templateContent, T model, Action<string> logCallback)
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

        globals.Import("f_Log", new Action<string>(logCallback));
    
        // // Special 'log' function so template can talk to your output
        // globals.Add("f_Log", new Func<string, object>(x=> { logCallback(x); return null; }));
        //
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