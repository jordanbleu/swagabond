using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Swagabond.Core.Mappers;
using Swagabond.IntegrationTests.Utils;
using Swagabond.ObjectModelV1;

namespace Swagabond.IntegrationTests;

public class CompositionSchemaTestsFixture : IAsyncLifetime
{
    public ApiV1 MappedApi { get; private set; } = ApiV1.Empty;

    public async Task InitializeAsync()
    {
        var services = ApiTransformerFactory.CreateV1Transformer();

        var mapper = services.GetRequiredService<OpenApiMapper>();
        var path = Path.Combine(AppContext.BaseDirectory, "SwaggerFiles/swagger_composition.json");
        await using var fs = new FileStream(path, FileMode.Open);
        MappedApi = await mapper.MapFromStreamV1(new()
        {
            FailOnDefinitionError = false,
            FailOnDefinitionWarning = false
        }, fs);
    }

    public Task DisposeAsync() => Task.CompletedTask;
}

public class CompositionSchemaTests : IClassFixture<CompositionSchemaTestsFixture>
{
    private readonly CompositionSchemaTestsFixture _fixture;

    public CompositionSchemaTests(CompositionSchemaTestsFixture fixture)
    {
        _fixture = fixture;
    }

    private SchemaDefinitionV1 GetThingProperty(string name) =>
        _fixture.MappedApi.Schemas.Single(s => s.Name == "Thing")
            .Properties.Single(p => p.OriginalName == name).Schema;

    private static void ShouldBeStatusEnum(SchemaDefinitionV1 schema)
    {
        schema.IsEnum.ShouldBeTrue();
        schema.Name.ShouldBe("Status");
        schema.ReferenceId.ShouldBe("Status");
        schema.DataType.ShouldBe(DataTypeV1.Int32);
        schema.EnumOptions.Select(o => o.Name).ShouldBe(new[] { "Active", "Inactive" });
    }

    [Fact]
    public void PlainEnumReference_IsUnchanged()
    {
        var schema = GetThingProperty("plainStatus");

        ShouldBeStatusEnum(schema);
        schema.Constraints.IsNullable.ShouldBeFalse();
    }

    [Theory]
    [InlineData("oneOfNullableStatus")]
    [InlineData("allOfNullableStatus")]
    public void NullableSingleBranchComposition_IsMappedAsNullableEnum(string propertyName)
    {
        var schema = GetThingProperty(propertyName);

        ShouldBeStatusEnum(schema);
        schema.Constraints.IsNullable.ShouldBeTrue();
    }

    [Fact]
    public void SingleBranchCompositionWithoutNullable_IsMappedAsNonNullableEnum()
    {
        var schema = GetThingProperty("anyOfStatus");

        ShouldBeStatusEnum(schema);
        schema.Constraints.IsNullable.ShouldBeFalse();
    }

    [Fact]
    public void SingleBranchComposition_UsesWrapperDescription()
    {
        GetThingProperty("allOfNullableStatus").Description.ShouldBe("Wrapper description");
    }

    [Theory]
    [InlineData("nullableWidget")]
    [InlineData("oneOfNullableWidget")]
    [InlineData("nullEnumBranchWidget")]
    public void NullableSingleBranchCompositionOfObject_IsMappedAsNullableObject(string propertyName)
    {
        var schema = GetThingProperty(propertyName);

        schema.Name.ShouldBe("Widget");
        schema.DataType.ShouldBe(DataTypeV1.Object);
        schema.Properties.Select(p => p.OriginalName).ShouldBe(new[] { "id" });
        schema.Constraints.IsNullable.ShouldBeTrue();
    }

    [Fact]
    public void ArrayOfSingleBranchComposition_IsMappedAsArrayOfEnum()
    {
        var schema = GetThingProperty("statusList");

        schema.IsArray.ShouldBeTrue();
        ShouldBeStatusEnum(schema);
    }

    [Theory]
    [InlineData("nullableWidgetArray", true, false)]
    [InlineData("nullableArrayOfNullableInts", true, true)]
    [InlineData("plainNullableWidgetArray", true, false)]
    [InlineData("arrayOfNullableInts", false, true)]
    public void Array_SeparatesArrayNullabilityFromItemNullability(string propertyName, bool arrayNullable, bool itemsNullable)
    {
        var schema = GetThingProperty(propertyName);

        schema.IsArray.ShouldBeTrue();
        schema.IsNullableArray.ShouldBe(arrayNullable);
        schema.Constraints.IsNullable.ShouldBe(itemsNullable);
    }

    [Fact]
    public void NullableWrappedArrayOfReference_KeepsReferencedItemType()
    {
        var schema = GetThingProperty("nullableWidgetArray");

        schema.Name.ShouldBe("Widget");
        schema.ReferenceId.ShouldBe("Widget");
        schema.DataType.ShouldBe(DataTypeV1.Object);
    }

    [Fact]
    public void MultiBranchOneOf_FallsBackToString()
    {
        var schema = GetThingProperty("union");

        schema.IsEnum.ShouldBeFalse();
        schema.Name.ShouldBe("String");
        schema.DataType.ShouldBe(DataTypeV1.String);
    }

    [Fact]
    public void ReferenceToWrapperComponent_KeepsComponentName()
    {
        var schema = GetThingProperty("refToWrapper");

        schema.Name.ShouldBe("NullableStatus");
        schema.IsEnum.ShouldBeFalse();
    }

    [Fact]
    public void WrapperComponent_KeepsItsOwnName()
    {
        _fixture.MappedApi.Schemas.Select(s => s.Name).ShouldBe(
            new[] { "Status", "NullableStatus", "Widget", "Thing" },
            ignoreOrder: true);
    }

    [Fact]
    public void NullableSingleBranchCompositionQueryParameter_IsMappedAsNullableEnum()
    {
        var param = _fixture.MappedApi.Operations.Single().QueryParameters.Single();

        ShouldBeStatusEnum(param.Schema);
        param.Schema.Constraints.IsNullable.ShouldBeTrue();
    }

    [Fact]
    public void NullableSingleBranchCompositionResponseBody_IsMappedAsWrappedObject()
    {
        var schema = _fixture.MappedApi.Operations.Single().SuccessResponseBody.Schema;

        schema.Name.ShouldBe("Thing");
        schema.DataType.ShouldBe(DataTypeV1.Object);
        schema.Constraints.IsNullable.ShouldBeTrue();
    }
}
