using AutoFixture;
using Microsoft.OpenApi.Models;
using Moq;
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

    [Fact]
    public void FromOpenApi_SelfReferencingSchema_DoesNotStackOverflow()
    {
        var autoMocker = new AutoMocker();

        autoMocker.GetMock<IDataTypeV1Transformer>()
            .Setup(x => x.FromOpenApi("object", It.IsAny<string>()))
            .Returns(DataTypeV1.Object);

        autoMocker.GetMock<IDataTypeV1Transformer>()
            .Setup(x => x.FromOpenApi("string", It.IsAny<string>()))
            .Returns(DataTypeV1.String);

        autoMocker.GetMock<ISchemaReferenceV1Transformer>()
            .Setup(x => x.FromOpenApi(It.IsAny<string>(), It.IsAny<SchemaDefinitionV1>(), It.IsAny<ApiV1>()))
            .Returns((string name, SchemaDefinitionV1 schema, ApiV1 _) =>
                new SchemaReferenceV1 { Name = name, Schema = schema, IsEmpty = false });

        var target = autoMocker.CreateInstance<SchemaDefinitionV1Transformer>();

        // ErrorResponse.errors is an array of ErrorResponse (circular)
        var selfRefSchema = new OpenApiSchema
        {
            Type = "object",
            Reference = new OpenApiReference { Id = "ErrorResponse", Type = ReferenceType.Schema },
            Properties = new Dictionary<string, OpenApiSchema>()
        };

        selfRefSchema.Properties["message"] = new OpenApiSchema { Type = "string" };
        selfRefSchema.Properties["errors"] = new OpenApiSchema
        {
            Type = "array",
            Items = selfRefSchema
        };

        var api = new ApiV1();

        var result = target.FromOpenApi(selfRefSchema, api);

        result.ShouldNotBeNull();
        result.IsEmpty.ShouldBeFalse();
        result.Properties.Count.ShouldBe(2);

        var errorsRef = result.Properties.First(p => p.Name == "errors");
        errorsRef.Schema.Properties.ShouldBeEmpty("circular ref should produce a stub with no nested properties");
    }
}