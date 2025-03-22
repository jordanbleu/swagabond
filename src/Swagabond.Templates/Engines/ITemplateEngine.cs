namespace Swagabond.Templates.Engines;

public interface ITemplateEngine
{
    Task<string> RenderTemplate<T>(string templateContent, T model, Action<string> logCallback);
    bool CanProcess(TemplateType templateType);
}