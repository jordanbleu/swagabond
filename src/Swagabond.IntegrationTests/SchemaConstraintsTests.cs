using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Swagabond.Core.Mappers;
using Swagabond.IntegrationTests.Utils;
using Swagabond.ObjectModelV1;

namespace Swagabond.IntegrationTests;

public class SchemaConstraintsTestsFixture : IAsyncLifetime
{
    public ApiV1 MappedApi { get; private set; }

    public async Task InitializeAsync()
    {
        var services = ApiTransformerFactory.CreateV1Transformer();
        var mapper = services.GetRequiredService<OpenApiMapper>();
        var path = Path.Combine(AppContext.BaseDirectory, "SwaggerFiles/swagger_constraints.json");
        var fs = new FileStream(path, FileMode.Open);
        MappedApi = await mapper.MapFromStreamV1(new()
        {
            FailOnDefinitionError = false,
            FailOnDefinitionWarning = false
        }, fs);
    }

    public Task DisposeAsync() => Task.CompletedTask;
}

[Category("integration_constraints")]
public class SchemaConstraintsTests : IClassFixture<SchemaConstraintsTestsFixture>
{
    private readonly SchemaConstraintsTestsFixture _fixture;

    public SchemaConstraintsTests(SchemaConstraintsTestsFixture fixture)
    {
        _fixture = fixture;
    }

    private SchemaReferenceV1 GetProp(string propName) =>
        _fixture.MappedApi.Schemas
            .First(s => s.ReferenceId == "CreateItemRequest")
            .Properties.First(p => p.OriginalName == propName);

    [Fact]
    public void StringProperty_MinLength_IsMapped()
    {
        var prop = GetProp("name");
        prop.Schema.Constraints.HasMinLength.ShouldBeTrue();
        prop.Schema.Constraints.MinLength.ShouldBe(1);
    }

    [Fact]
    public void StringProperty_MaxLength_IsMapped()
    {
        var prop = GetProp("name");
        prop.Schema.Constraints.HasMaxLength.ShouldBeTrue();
        prop.Schema.Constraints.MaxLength.ShouldBe(100);
    }

    [Fact]
    public void StringProperty_Pattern_IsMapped()
    {
        var prop = GetProp("name");
        prop.Schema.Constraints.Pattern.ShouldBe("^[a-zA-Z0-9 ]+$");
    }

    [Fact]
    public void IntegerProperty_MinValue_IsMapped()
    {
        var prop = GetProp("quantity");
        prop.Schema.Constraints.HasMinValue.ShouldBeTrue();
        prop.Schema.Constraints.MinValue.ShouldBe(1m);
    }

    [Fact]
    public void IntegerProperty_MaxValue_IsMapped()
    {
        var prop = GetProp("quantity");
        prop.Schema.Constraints.HasMaxValue.ShouldBeTrue();
        prop.Schema.Constraints.MaxValue.ShouldBe(999m);
    }

    [Fact]
    public void IntegerProperty_DefaultInclusive_BothInclusive()
    {
        // quantity has no exclusiveMinimum/exclusiveMaximum so both should be inclusive
        var prop = GetProp("quantity");
        prop.Schema.Constraints.IsMinValueInclusive.ShouldBeTrue();
        prop.Schema.Constraints.IsMaxValueInclusive.ShouldBeTrue();
    }

    [Fact]
    public void DoubleProperty_ExclusiveMinimum_IsNotInclusive()
    {
        // price has exclusiveMinimum: true
        var prop = GetProp("price");
        prop.Schema.Constraints.HasMinValue.ShouldBeTrue();
        prop.Schema.Constraints.MinValue.ShouldBe(0.01m);
        prop.Schema.Constraints.IsMinValueInclusive.ShouldBeFalse();
    }

    [Fact]
    public void DoubleProperty_ExclusiveMaximumFalse_IsInclusive()
    {
        // price has exclusiveMaximum: false
        var prop = GetProp("price");
        prop.Schema.Constraints.HasMaxValue.ShouldBeTrue();
        prop.Schema.Constraints.MaxValue.ShouldBe(99999.99m);
        prop.Schema.Constraints.IsMaxValueInclusive.ShouldBeTrue();
    }

    [Fact]
    public void FloatProperty_ExclusiveMaximum_IsNotInclusive()
    {
        // rating has exclusiveMaximum: true
        var prop = GetProp("rating");
        prop.Schema.Constraints.HasMaxValue.ShouldBeTrue();
        prop.Schema.Constraints.MaxValue.ShouldBe(5m);
        prop.Schema.Constraints.IsMaxValueInclusive.ShouldBeFalse();
    }

    [Fact]
    public void FloatProperty_ExclusiveMinimumFalse_IsInclusive()
    {
        // rating has exclusiveMinimum: false
        var prop = GetProp("rating");
        prop.Schema.Constraints.HasMinValue.ShouldBeTrue();
        prop.Schema.Constraints.MinValue.ShouldBe(0m);
        prop.Schema.Constraints.IsMinValueInclusive.ShouldBeTrue();
    }

    [Fact]
    public void NullableString_IsNullable()
    {
        var prop = GetProp("description");
        prop.Schema.Constraints.IsNullable.ShouldBeTrue();
    }

    [Fact]
    public void NullableString_AlsoHasMaxLength()
    {
        var prop = GetProp("description");
        prop.Schema.Constraints.HasMaxLength.ShouldBeTrue();
        prop.Schema.Constraints.MaxLength.ShouldBe(500);
    }

    [Fact]
    public void UnconstrainedProperty_HasNoConstraints()
    {
        var prop = GetProp("unconstrained");
        prop.Schema.Constraints.HasMinValue.ShouldBeFalse();
        prop.Schema.Constraints.HasMaxValue.ShouldBeFalse();
        prop.Schema.Constraints.HasMinLength.ShouldBeFalse();
        prop.Schema.Constraints.HasMaxLength.ShouldBeFalse();
        prop.Schema.Constraints.Pattern.ShouldBeEmpty();
        prop.Schema.Constraints.IsNullable.ShouldBeFalse();
    }

    [Fact]
    public void ConstrainedProperty_IsEmptyFalse()
    {
        // IsEmpty should be false on any schema we've touched (constraints object always created)
        GetProp("name").Schema.Constraints.IsEmpty.ShouldBeFalse();
    }
}
