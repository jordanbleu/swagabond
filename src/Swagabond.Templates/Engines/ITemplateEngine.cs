namespace Swagabond.Templates.Engines;

public interface ITemplateEngine
{
    Task<string> RenderTemplate<T>(string templateContent, T model);
    bool CanProcess(TemplateType templateType);
}