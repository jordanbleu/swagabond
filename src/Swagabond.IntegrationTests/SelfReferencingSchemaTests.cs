using System.ComponentModel;
using Shouldly;
using Swagabond.ObjectModelV1;

namespace Swagabond.IntegrationTests;

/// <summary>
/// Regression tests for specs containing self-referencing schemas (e.g. ErrorResponse.errors -> ErrorResponse).
/// These previously caused a StackOverflowException during transformation.
/// Both swagger fixtures now include an ErrorResponse schema that references itself.
/// </summary>
public class SelfReferencingSchemaTests_OpenApi3 : IClassFixture<Swagger3MapperTestsFixture>
{
    private readonly Swagger3MapperTestsFixture _fixture;

    public SelfReferencingSchemaTests_OpenApi3(Swagger3MapperTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    [Category("integration_openapi_3")]
    public void SelfReferencingSchema_ParsesWithoutStackOverflow()
    {
        var errorSchema = _fixture.MappedApi.Schemas
            .FirstOrDefault(s => s.ReferenceId == "ErrorResponse");

        errorSchema.ShouldNotBeNull("the self-referencing ErrorResponse schema should be parsed");
        errorSchema.DataType.ShouldBe(DataTypeV1.Object);
        errorSchema.Properties.ShouldNotBeEmpty("non-circular properties should still be mapped");

        var errorsProperty = errorSchema.Properties
            .FirstOrDefault(p => p.OriginalName == "errors");

        errorsProperty.ShouldNotBeNull();
        errorsProperty.Schema.Properties.ShouldBeEmpty(
            "the circular back-reference should produce a stub with no nested properties");
    }
}

public class SelfReferencingSchemaTests_OpenApi2 : IClassFixture<Swagger2MapperTestsFixture>
{
    private readonly Swagger2MapperTestsFixture _fixture;

    public SelfReferencingSchemaTests_OpenApi2(Swagger2MapperTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    [Category("integration_openapi_2")]
    public void SelfReferencingSchema_ParsesWithoutStackOverflow()
    {
        var errorSchema = _fixture.MappedApi.Schemas
            .FirstOrDefault(s => s.ReferenceId == "ErrorResponse");

        errorSchema.ShouldNotBeNull("the self-referencing ErrorResponse schema should be parsed");
        errorSchema.DataType.ShouldBe(DataTypeV1.Object);
        errorSchema.Properties.ShouldNotBeEmpty("non-circular properties should still be mapped");

        var errorsProperty = errorSchema.Properties
            .FirstOrDefault(p => p.OriginalName == "errors");

        errorsProperty.ShouldNotBeNull();
        errorsProperty.Schema.Properties.ShouldBeEmpty(
            "the circular back-reference should produce a stub with no nested properties");
    }
}
