using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SampleWebApi.Swagger;

public class ExtensionOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        
        operation.Extensions.Add("x-operationExtension", new OpenApiString("hello world"));
    }
}