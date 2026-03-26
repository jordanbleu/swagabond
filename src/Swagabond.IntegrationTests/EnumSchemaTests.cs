using System.ComponentModel;
using Shouldly;
using Swagabond.ObjectModelV1;

namespace Swagabond.IntegrationTests;

public class EnumSchemaTests_OpenApi3 : IClassFixture<Swagger3MapperTestsFixture>
{
    private readonly Swagger3MapperTestsFixture _fixture;

    public EnumSchemaTests_OpenApi3(Swagger3MapperTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    [Category("integration_openapi_3")]
    public void OrderStatusEnum_IsMappedCorrectly()
    {
        var orderSchema = _fixture.MappedApi.Schemas
            .First(s => s.ReferenceId == "Order");

        var statusProp = orderSchema.Properties
            .First(p => p.OriginalName == "status");

        statusProp.Schema.IsEnum.ShouldBeTrue();
        statusProp.Schema.EnumOptions.Count.ShouldBe(3);
        statusProp.Schema.EnumOptions.Select(e => e.Value)
            .ShouldBe(new[] { "placed", "approved", "delivered" }, ignoreOrder: false);
    }

    [Fact]
    [Category("integration_openapi_3")]
    public void PetStatusEnum_IsMappedCorrectly()
    {
        var petSchema = _fixture.MappedApi.Schemas
            .First(s => s.ReferenceId == "Pet");

        var statusProp = petSchema.Properties
            .First(p => p.OriginalName == "status");

        statusProp.Schema.IsEnum.ShouldBeTrue();
        statusProp.Schema.EnumOptions.Count.ShouldBe(3);
        statusProp.Schema.EnumOptions.Select(e => e.Value)
            .ShouldBe(new[] { "available", "pending", "sold" }, ignoreOrder: false);
    }

    [Fact]
    [Category("integration_openapi_3")]
    public void EnumWithoutNameExtension_GeneratesItemNNames()
    {
        var orderSchema = _fixture.MappedApi.Schemas
            .First(s => s.ReferenceId == "Order");

        var statusProp = orderSchema.Properties
            .First(p => p.OriginalName == "status");

        statusProp.Schema.EnumOptions.Select(e => e.Name)
            .ShouldBe(new[] { "Item0", "Item1", "Item2" }, ignoreOrder: false);
    }
}

public class EnumSchemaTests_OpenApi2 : IClassFixture<Swagger2MapperTestsFixture>
{
    private readonly Swagger2MapperTestsFixture _fixture;

    public EnumSchemaTests_OpenApi2(Swagger2MapperTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    [Category("integration_openapi_2")]
    public void OrderStatusEnum_IsMappedCorrectly()
    {
        var orderSchema = _fixture.MappedApi.Schemas
            .First(s => s.ReferenceId == "Order");

        var statusProp = orderSchema.Properties
            .First(p => p.OriginalName == "status");

        statusProp.Schema.IsEnum.ShouldBeTrue();
        statusProp.Schema.EnumOptions.Count.ShouldBe(3);
        statusProp.Schema.EnumOptions.Select(e => e.Value)
            .ShouldBe(new[] { "placed", "approved", "delivered" }, ignoreOrder: false);
    }

    [Fact]
    [Category("integration_openapi_2")]
    public void PetStatusEnum_IsMappedCorrectly()
    {
        var petSchema = _fixture.MappedApi.Schemas
            .First(s => s.ReferenceId == "Pet");

        var statusProp = petSchema.Properties
            .First(p => p.OriginalName == "status");

        statusProp.Schema.IsEnum.ShouldBeTrue();
        statusProp.Schema.EnumOptions.Count.ShouldBe(3);
        statusProp.Schema.EnumOptions.Select(e => e.Value)
            .ShouldBe(new[] { "available", "pending", "sold" }, ignoreOrder: false);
    }
}
