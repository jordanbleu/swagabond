using AutoFixture;
using Microsoft.OpenApi.Models;
using Moq.AutoMock;
using Shouldly;
using Swagabond.ObjectModelV1;
using Swagabond.ObjectModelV1.Transformer;
using Swagabond.Tests.Extensions;

namespace Swagabond.Tests.ObjectModelV1;

public class SchemaDefinitionV1TransformerTests
{
    [Fact]
    public void FromOpenApi_MapsPropertyConstraints_Properly()
    {
        var fixture = new Fixture().WithOpenApiConfigured();
        var schema = new OpenApiSchema()
        {
            Minimum = 1,
            Maximum = 10,
            MinLength = 100,
            MaxLength = 1000,
            ExclusiveMaximum = false,
            ExclusiveMinimum = true,
            Pattern = "123",
            Nullable = true
        };
        
        var api = fixture.Create<ApiV1>();

        var autoMocker = new AutoMocker();

        var target = autoMocker.CreateInstance<SchemaDefinitionV1Transformer>();

        var result = target.FromOpenApi(schema, api);

        var constraints = result.Constraints;
        constraints.IsEmpty.ShouldBeFalse();
        constraints.MaxLength.ShouldBe(1000);
        constraints.MinLength.ShouldBe(100);
        constraints.HasMinLength.ShouldBeTrue();
        constraints.HasMaxLength.ShouldBeTrue();
        constraints.MinValue.ShouldBe(1);
        constraints.MaxValue.ShouldBe(10);
        constraints.IsMinValueInclusive.ShouldBe(false);
        constraints.IsMaxValueInclusive.ShouldBe(true);
        constraints.HasMaxValue.ShouldBeTrue();
        constraints.HasMinValue.ShouldBeTrue();
        constraints.Pattern.ShouldBe("123");
        constraints.IsNullable.ShouldBe(true);

    }
}