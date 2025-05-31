using System.Collections;
using System.Reflection;
using LoxSmoke.DocXml;
using Scriban;
using Swagabond.ObjectModelV1;
using Swagabond.Templates.Engines;
using Swagabond.Templates.Functions;

namespace ObjectModelDocGenerator;

public class Program
{
    private static List<string> ProcessedTypes = new List<string>();
    
    private static readonly string ObjectModelTemplateFile = "Templates/readme_template.scriban";
    private static readonly string TemplateFunctionsTemplateFile = "Templates/templatefunctions_template.scriban";

    
    private static readonly string ObjectModelFile = "../../../../../../src/Swagabond.ObjectModelV1/bin/debug/net8.0/Swagabond.ObjectModelV1.xml";
    private static readonly string TemplatesFile = "../../../../../../src/Swagabond.Templates/bin/debug/net8.0/Swagabond.Templates.xml";


    public static async Task Main(string[] args)
    {
        Console.WriteLine("Current Dir: " + Directory.GetCurrentDirectory());
        if (!File.Exists(ObjectModelTemplateFile))
            throw new Exception("Missing object model template file.");
        
        if (!File.Exists(TemplateFunctionsTemplateFile))
            throw new Exception("Missing template functions template file.");
               
        if (!File.Exists(ObjectModelFile))
            throw new Exception("Make sure the Swagabond.ObjectModelV1 project is built before running this tool.");

        var docXmlReader = new DocXmlReader(ObjectModelFile);

        var objectModelTemplateContent = await File.ReadAllTextAsync(ObjectModelTemplateFile);
        
        await GenerateMarkdownForObjectModel(typeof(ApiV1), docXmlReader, objectModelTemplateContent);
        await GenerateMarkdownForTemplateFunctions();

        Console.WriteLine("****");
        Console.WriteLine("Doc generation is done. Thank you for clicking the button");
    }

    private static async Task GenerateMarkdownForTemplateFunctions()
    {
        var reader = new DocXmlReader(TemplatesFile);
        
        var data = new TemplateFunctionsData();
        
        // the first one is hard coded because it isn't defined on TemplateFunctions
        data.Functions.Add(new TemplateFunction()
        {
            Name = "f_Log",
            Comment = "Used for debugging. Will log a message to the console or the configured logger.",
            Params = new List<TemplateFunctionParam>()
            {
                new()
                {
                    Name = "message",
                    Comment = "The message to log."
                }
            }
        });

        var templateFuncs = typeof(TemplateFunctions).GetMethods(BindingFlags.Public | BindingFlags.Static);

        foreach (var templateFunc in templateFuncs)
        {
            var cmt = reader.GetMethodComments(templateFunc);
            
            var templateFuncData = new TemplateFunction()
            {
                Comment = cmt.Summary,
                Name = "f_" + templateFunc.Name,
            };
            
            foreach (var asdf in cmt.Parameters)
            {
                templateFuncData.Params.Add(new TemplateFunctionParam()
                {
                    Name = asdf.Name,
                    Comment = asdf.Text
                });
            }
            
            data.Functions.Add(templateFuncData);
        }

        var templateContent = await File.ReadAllTextAsync(TemplateFunctionsTemplateFile);
        
        // Here we are actually using our very own template renderer to render the template
        // for writing templates for our template renderer. Absolute insanity!
        var templateEngine = new ScribanTemplateEngine();
        var contents = await templateEngine.RenderTemplate(templateContent, data, LogCallback);
        
        await File.WriteAllTextAsync($"../../../../../../docs/TemplateFunctions.md", contents);
    }

    public class TemplateFunctionsData
    {
        public List<TemplateFunction> Functions { get; set; } = new();
    }

    public class TemplateFunction()
    {
        public string Name { get; set; }
        public string Comment { get; set; }
        public List<TemplateFunctionParam> Params { get; set; } = new();
    }

    public class TemplateFunctionParam
    {
        public string Name { get; set; }
        public string Comment { get; set; }
    }


    private static async Task GenerateMarkdownForObjectModel(Type t, DocXmlReader reader, string templateContent)
    {
        if (t.FullName == null || ProcessedTypes.Contains(t.FullName))
        {
            return;
        }

        ProcessedTypes.Add(t.FullName);

        var classComment = reader.GetTypeComments(t);
        var typeComment = classComment.Summary.RemoveNewlines();
        ClassData data;
        
        // enums are a whole separate thing lol
        if (t.IsEnum)
        {
            data = new ClassData()
            {
                Name = t.Name,
                IsEnum = true,
                Comment = typeComment.RemoveNewlines()
            };

            foreach (var enumValue in Enum.GetValues(t))
            {
                var enumComment = reader.GetEnumComments(t, true);
                var valueComment = enumComment.ValueComments.FirstOrDefault(x => x.Value == (int)enumValue);
                var enumValueName = Enum.GetName(t, enumValue) ?? string.Empty;
                var enumValueSummary = valueComment?.Summary ?? String.Empty;
                
                data.EnumValues.Add(new()
                {
                    Name = enumValueName,
                    Comment = enumValueSummary.RemoveNewlines(),
                });
            }

        }
        else 
        {

            data = new ClassData()
            {
                Name = t.Name,
                Comment = typeComment.RemoveNewlines(),
                Properties = new List<PropertyData>()
            };

            var props = t.GetProperties(BindingFlags.Public | BindingFlags.Instance).OrderBy(x=>x.Name);
            foreach (var prop in props)
            {
                var propComment = reader.GetMemberComments(prop);

                var propType = prop.PropertyType;

                PropertyData propertyData = null;

                var propName = prop.Name;

                // This is a collection type (except strings)
                if (propType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(propType))
                {
                    // This is a dictionary 
                    if (typeof(IDictionary).IsAssignableFrom(propType))
                    {
                        propertyData = new PropertyData()
                        {
                            Name = propName,
                            Comment = propComment.Summary.RemoveNewlines(),
                            IsArray = false,
                            IsDictionary = true,
                            IsPrimitive = true,
                            PropertyTypeName = "Dynamic"
                        };
                    }
                    else
                    {
                        // This is an ICollection<T> probably 
                        var underlyingType = propType.GetGenericArguments().First();
                        propertyData = new PropertyData()
                        {
                            Name = propName,
                            IsArray = true,
                            Comment = propComment.Summary.RemoveNewlines(),
                            IsDictionary = false,
                            IsPrimitive = underlyingType.IsPrimitive || underlyingType == typeof(string),
                            Example = "",
                            PropertyTypeName = underlyingType.Name,
                        };

                        if (!propType.IsPrimitive && propType != typeof(string))
                        {
                            // recursively also map a new file for the property
                            await GenerateMarkdownForObjectModel(underlyingType, reader, templateContent);
                        }

                    }
                }
                else
                {
                    // This is a normal property with either a primitive type or a complex type
                    propertyData = new PropertyData()
                    {
                        Name = propName,
                        IsArray = false,
                        Comment = propComment.Summary.RemoveNewlines(),
                        IsDictionary = false,
                        IsPrimitive = propType.IsPrimitive || propType == typeof(string),
                        PropertyTypeName = prop.PropertyType.Name,
                        Example = propComment.Example
                    };

                    if (!propType.IsPrimitive && propType != typeof(string))
                    {
                        // recursively also map a new file for the property
                        await GenerateMarkdownForObjectModel(propType, reader, templateContent);
                    }
                }

                data.Properties.Add(propertyData);

            }
        }

        // Here we are actually using our very own template renderer to render the template
        // for writing templates for our template renderer. Absolute insanity!
        var templateEngine = new ScribanTemplateEngine();
        var contents = await templateEngine.RenderTemplate(templateContent, data, LogCallback);
        
        await File.WriteAllTextAsync($"../../../../../../docs/{data.Name}.md", contents);

    }
    
    private static void LogCallback(string s) => Console.WriteLine(s);
}

public class ClassData
{
    public string Name { get; set; }

    public string Comment { get; set; }

    public List<PropertyData> Properties { get; set; } = new();

    public bool IsEnum { get; set; } = false;

    public List<EnumOption> EnumValues { get; set; } = new List<EnumOption>();
    
}

public class PropertyData
{
    public string Name { get; set; }
    public string Comment { get; set; }
    public bool IsPrimitive { get; set; }
    public string Example { get; set; }
    public bool IsArray { get; set; }
    public bool IsDictionary { get; set; }
    public string PropertyTypeName { get; set; } = string.Empty;

}

/// Template writers only care about the enum names, not the underlying value
public class EnumOption
{
    public string Name { get; set; }
    public string Comment { get; set; }
}