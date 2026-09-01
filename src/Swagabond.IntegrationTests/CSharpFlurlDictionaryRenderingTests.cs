using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Swagabond.Core.Mappers;
using Swagabond.IntegrationTests.Utils;
using Swagabond.ObjectModelV1;
using Swagabond.Templates;

namespace Swagabond.IntegrationTests;

public class CSharpFlurlDictionaryRenderingTestsFixture : IAsyncLifetime
{
    public string RenderedPlayer { get; private set; } = string.Empty;
    public string RenderedScoresByUser { get; private set; } = string.Empty;
    public string RenderedOpenAccount { get; private set; } = string.Empty;
    public string RenderedClosedAccount { get; private set; } = string.Empty;

    public async Task InitializeAsync()
    {
        var services = ApiTransformerFactory.CreateV1Transformer();

        var mapper = services.GetRequiredService<OpenApiMapper>();
        var path = Path.Combine(AppContext.BaseDirectory, "SwaggerFiles/swagger_dictionary.json");
        await using var fs = new FileStream(path, FileMode.Open);
        var api = await mapper.MapFromStreamV1(new()
        {
            FailOnDefinitionError = false,
            FailOnDefinitionWarning = false
        }, fs);

        var templateEngine = services.GetRequiredService<TemplateEngineFactory>().GetEngine(TemplateType.Scriban);
        var functions = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Templates/csharp-flurl/functions.scriban"));
        var schemaTemplate = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Templates/csharp-flurl/schema.scriban"));
        var fullTemplate = functions + "\n" + schemaTemplate + "\n";

        var player = api.Schemas.Single(s => s.Name == "Player");
        var scoresByUser = api.Schemas.Single(s => s.Name == "ScoresByUser");
        var openAccount = api.Schemas.Single(s => s.Name == "OpenAccount");
        var closedAccount = api.Schemas.Single(s => s.Name == "ClosedAccount");

        RenderedPlayer = await templateEngine.RenderTemplate(fullTemplate, player, _ => { });
        RenderedScoresByUser = await templateEngine.RenderTemplate(fullTemplate, scoresByUser, _ => { });
        RenderedOpenAccount = await templateEngine.RenderTemplate(fullTemplate, openAccount, _ => { });
        RenderedClosedAccount = await templateEngine.RenderTemplate(fullTemplate, closedAccount, _ => { });
    }

    public Task DisposeAsync() => Task.CompletedTask;
}

public class CSharpFlurlDictionaryRenderingTests : IClassFixture<CSharpFlurlDictionaryRenderingTestsFixture>
{
    private readonly CSharpFlurlDictionaryRenderingTestsFixture _fixture;

    public CSharpFlurlDictionaryRenderingTests(CSharpFlurlDictionaryRenderingTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void DictionaryProperty_RendersAsGenericDictionaryOfReferencedType()
    {
        _fixture.RenderedPlayer.ShouldContain("public Dictionary<string, Attribute> Attributes { get; set; }");
    }

    [Fact]
    public void TopLevelDictionarySchema_DoesNotEmitAStandaloneType()
    {
        _fixture.RenderedScoresByUser.ShouldNotContain("public record");
        _fixture.RenderedScoresByUser.ShouldNotContain("public enum");
    }

    [Fact]
    public void ObjectWithFixedAndAdditionalProperties_RendersFixedPropertiesPlusAdditionalPropertiesBag()
    {
        _fixture.RenderedOpenAccount.ShouldContain("public record OpenAccount");
        _fixture.RenderedOpenAccount.ShouldContain("public string Id { get; set; }");
        _fixture.RenderedOpenAccount.ShouldContain("public string Name { get; set; }");
        _fixture.RenderedOpenAccount.ShouldContain("public Dictionary<string, Attribute> AdditionalProperties { get; set; }");
    }

    [Fact]
    public void ObjectWithAdditionalPropertiesExplicitlyFalse_RendersOnlyFixedProperties()
    {
        _fixture.RenderedClosedAccount.ShouldContain("public record ClosedAccount");
        _fixture.RenderedClosedAccount.ShouldContain("public string Id { get; set; }");
        _fixture.RenderedClosedAccount.ShouldNotContain("AdditionalProperties");
    }
}
