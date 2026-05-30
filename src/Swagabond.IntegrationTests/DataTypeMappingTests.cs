using System.ComponentModel;
using Shouldly;
using Swagabond.ObjectModelV1;

namespace Swagabond.IntegrationTests;

/// <summary>
/// Verifies that each DataTypeV1 value is correctly detected from OpenAPI type/format combinations.
/// Uses the petstore (v2 and v3) and sample API fixtures.
/// </summary>
[Category("integration_openapi_3")]
public class DataTypeMappingTests_OpenApi3 : IClassFixture<Swagger3MapperTestsFixture>
{
    private readonly Swagger3MapperTestsFixture _fixture;

    public DataTypeMappingTests_OpenApi3(Swagger3MapperTestsFixture fixture)
    {
        _fixture = fixture;
    }

    private SchemaDefinitionV1 GetSchema(string referenceId) =>
        _fixture.MappedApi.Schemas.First(s => s.ReferenceId == referenceId);

    private SchemaReferenceV1 GetProp(string schemaId, string propName) =>
        GetSchema(schemaId).Properties.First(p => p.OriginalName == propName);

    [Fact]
    public void IntegerInt64Format_MapsToInt64()
    {
        GetProp("Order", "id").Schema.DataType.ShouldBe(DataTypeV1.Int64);
        GetProp("Order", "petId").Schema.DataType.ShouldBe(DataTypeV1.Int64);
    }

    [Fact]
    public void IntegerInt32Format_MapsToInt32()
    {
        GetProp("Order", "quantity").Schema.DataType.ShouldBe(DataTypeV1.Int32);
    }

    [Fact]
    public void StringDateTimeFormat_MapsToDateTime()
    {
        GetProp("Order", "shipDate").Schema.DataType.ShouldBe(DataTypeV1.DateTime);
    }

    [Fact]
    public void Boolean_MapsToBoolean()
    {
        GetProp("Order", "complete").Schema.DataType.ShouldBe(DataTypeV1.Boolean);
    }

    [Fact]
    public void PlainString_MapsToString()
    {
        GetProp("User", "username").Schema.DataType.ShouldBe(DataTypeV1.String);
        GetProp("Pet", "name").Schema.DataType.ShouldBe(DataTypeV1.String);
    }

    [Fact]
    public void ObjectRef_MapsToObject()
    {
        GetSchema("Pet").DataType.ShouldBe(DataTypeV1.Object);
        GetSchema("Order").DataType.ShouldBe(DataTypeV1.Object);
    }

    [Fact]
    public void ArrayProperty_IsMarkedAsArray_WithCorrectItemType()
    {
        var photoUrls = GetProp("Pet", "photoUrls");
        photoUrls.Schema.IsArray.ShouldBeTrue();
        photoUrls.Schema.DataType.ShouldBe(DataTypeV1.String);
        photoUrls.Schema.IsPrimitive.ShouldBeTrue();
    }

    [Fact]
    public void ArrayOfObjects_IsMarkedAsArray_WithObjectItemType()
    {
        var tags = GetProp("Pet", "tags");
        tags.Schema.IsArray.ShouldBeTrue();
        tags.Schema.DataType.ShouldBe(DataTypeV1.Object);
    }

    [Fact]
    public void IsPrimitive_FalseForObject_TrueForPrimitives()
    {
        GetSchema("Pet").IsPrimitive.ShouldBeFalse();
        GetProp("Order", "quantity").Schema.IsPrimitive.ShouldBeTrue();
        GetProp("Order", "shipDate").Schema.IsPrimitive.ShouldBeTrue();
        GetProp("Order", "complete").Schema.IsPrimitive.ShouldBeTrue();
    }
}

[Category("integration_openapi_2")]
public class DataTypeMappingTests_OpenApi2 : IClassFixture<Swagger2MapperTestsFixture>
{
    private readonly Swagger2MapperTestsFixture _fixture;

    public DataTypeMappingTests_OpenApi2(Swagger2MapperTestsFixture fixture)
    {
        _fixture = fixture;
    }

    private SchemaDefinitionV1 GetSchema(string referenceId) =>
        _fixture.MappedApi.Schemas.First(s => s.ReferenceId == referenceId);

    private SchemaReferenceV1 GetProp(string schemaId, string propName) =>
        GetSchema(schemaId).Properties.First(p => p.OriginalName == propName);

    [Fact]
    public void IntegerInt64Format_MapsToInt64()
    {
        GetProp("Order", "id").Schema.DataType.ShouldBe(DataTypeV1.Int64);
    }

    [Fact]
    public void IntegerInt32Format_MapsToInt32()
    {
        GetProp("Order", "quantity").Schema.DataType.ShouldBe(DataTypeV1.Int32);
    }

    [Fact]
    public void StringDateTimeFormat_MapsToDateTime()
    {
        GetProp("Order", "shipDate").Schema.DataType.ShouldBe(DataTypeV1.DateTime);
    }

    [Fact]
    public void Boolean_MapsToBoolean()
    {
        GetProp("Order", "complete").Schema.DataType.ShouldBe(DataTypeV1.Boolean);
    }
}

[Category("integration_sample")]
public class DataTypeMappingTests_SampleApi : IClassFixture<SampleApiMapperTestsFixture>
{
    private readonly SampleApiMapperTestsFixture _fixture;

    public DataTypeMappingTests_SampleApi(SampleApiMapperTestsFixture fixture)
    {
        _fixture = fixture;
    }

    private SchemaDefinitionV1 GetSchema(string referenceId) =>
        _fixture.MappedApi.Schemas.First(s => s.ReferenceId == referenceId);

    private SchemaReferenceV1 GetProp(string schemaId, string propName) =>
        GetSchema(schemaId).Properties.First(p => p.OriginalName == propName);

    [Fact]
    public void StringUuidFormat_MapsToGuid()
    {
        GetProp("SampleWebApi.Controllers.FranchiseGetResponseItem", "id").Schema.DataType.ShouldBe(DataTypeV1.Guid);
        GetProp("SampleWebApi.Controllers.MenuItemResponseItem", "id").Schema.DataType.ShouldBe(DataTypeV1.Guid);
        GetProp("SampleWebApi.Controllers.RestaurantGetResponseItem", "id").Schema.DataType.ShouldBe(DataTypeV1.Guid);
    }

    [Fact]
    public void IntegerInt32Format_MapsToInt32()
    {
        GetProp("SampleWebApi.Controllers.RestaurantGetResponseItem", "storeNumber").Schema.DataType.ShouldBe(DataTypeV1.Int32);
        GetProp("SampleWebApi.Controllers.MenutItemNutritionFacts", "calories").Schema.DataType.ShouldBe(DataTypeV1.Int32);
    }
}
