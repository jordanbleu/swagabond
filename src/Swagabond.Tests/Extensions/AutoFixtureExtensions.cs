using AutoFixture;
using AutoFixture.Kernel;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;

namespace Swagabond.Tests.Extensions;

public static class AutoFixtureExtensions
{
    /// <summary>
    /// Configures the AutoFixture <see cref="Fixture"/> to support generation of OpenAPI-compatible objects.
    /// </summary>
    /// <param name="fixture">The fixture to configure.</param>
    public static Fixture WithOpenApiConfigured(this Fixture fixture)
    {
        // Credit: ChatGPT
        // Prevent circular reference crashes
        fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior(recursionDepth: 1));

        // Type relays so interfaces and abstract types can be instantiated
        fixture.Customizations.Add(new TypeRelay(typeof(IOpenApiAny), typeof(OpenApiString)));
        fixture.Customizations.Add(new TypeRelay(typeof(IOpenApiExtension), typeof(OpenApiString)));

        // Custom builder to handle complex OpenApiDocument structure
        fixture.Customizations.Add(new OpenApiDocumentBuilder());

        return fixture;
    }

    
}

public class OpenApiDocumentBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is not Type t || t != typeof(OpenApiDocument))
            return new NoSpecimen();

        return new OpenApiDocument
        {
            Info = context.Create<OpenApiInfo>(),
            Paths = new OpenApiPaths(),
            Components = new OpenApiComponents(),
            Tags = new List<OpenApiTag> { context.Create<OpenApiTag>() },
            Servers = new List<OpenApiServer> { context.Create<OpenApiServer>() }
        };
    }
}