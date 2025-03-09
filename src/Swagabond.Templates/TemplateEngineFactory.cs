using Swagabond.Templates.Engines;

namespace Swagabond.Templates;

public class TemplateEngineFactory
{
    private readonly IEnumerable<ITemplateEngine> _templateEngines;

    public TemplateEngineFactory(IEnumerable<ITemplateEngine> templateEngines)
    {
        _templateEngines = templateEngines;
    }


    public ITemplateEngine GetEngine(TemplateType templateType)
    {
        var engine = _templateEngines.FirstOrDefault(e => e.CanProcess(templateType));
        
        if (engine == null)
        {
            throw new NotSupportedException($"No template engine found for {templateType}");
        }

        return engine;
    }

}