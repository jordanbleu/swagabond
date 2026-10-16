using AutoFixture;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
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

    [Fact]
    public void FromOpenApi_AdditionalPropertiesWithSchemaAndNoProperties_IsMappedAsDictionary()
    {
        var autoMocker = new AutoMocker();

        autoMocker.GetMock<IDataTypeV1Transformer>()
            .Setup(x => x.FromOpenApi("object", It.IsAny<string>()))
            .Returns(DataTypeV1.Object);

        autoMocker.GetMock<IDataTypeV1Transformer>()
            .Setup(x => x.FromOpenApi("string", It.IsAny<string>()))
            .Returns(DataTypeV1.String);

        var target = autoMocker.CreateInstance<SchemaDefinitionV1Transformer>();

        var schema = new OpenApiSchema
        {
            Type = "object",
            Properties = new Dictionary<string, OpenApiSchema>(),
            AdditionalPropertiesAllowed = true,
            AdditionalProperties = new OpenApiSchema { Type = "string" }
        };

        var result = target.FromOpenApi(schema, new ApiV1());

        result.IsDictionary.ShouldBeTrue();
        result.DataType.ShouldBe(DataTypeV1.Object);
        result.Properties.ShouldBeEmpty();
        result.AdditionalPropertiesSchema.DataType.ShouldBe(DataTypeV1.String);
    }

    [Fact]
    public void FromOpenApi_ObjectWithBothPropertiesAndAdditionalProperties_IsNotADictionary_ButKeepsAdditionalPropertiesSchema()
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

        var schema = new OpenApiSchema
        {
            Type = "object",
            Properties = new Dictionary<string, OpenApiSchema> { ["id"] = new OpenApiSchema { Type = "string" } },
            AdditionalPropertiesAllowed = true,
            AdditionalProperties = new OpenApiSchema { Type = "string" }
        };

        var result = target.FromOpenApi(schema, new ApiV1());

        result.IsDictionary.ShouldBeFalse();
        result.Properties.Count.ShouldBe(1);
        result.AdditionalPropertiesSchema.IsEmpty.ShouldBeFalse();
        result.AdditionalPropertiesSchema.DataType.ShouldBe(DataTypeV1.String);
    }

    [Fact]
    public void FromOpenApi_EmptyObjectWithNoAdditionalProperties_IsNotMappedAsDictionary()
    {
        // AdditionalPropertiesAllowed defaults to true with no AdditionalProperties schema whether
        // additionalProperties: true was explicitly set or the keyword was omitted entirely - the two
        // are indistinguishable, so a bare empty object must not be reclassified as a dictionary.
        var autoMocker = new AutoMocker();

        autoMocker.GetMock<IDataTypeV1Transformer>()
            .Setup(x => x.FromOpenApi("object", It.IsAny<string>()))
            .Returns(DataTypeV1.Object);

        var target = autoMocker.CreateInstance<SchemaDefinitionV1Transformer>();

        var schema = new OpenApiSchema
        {
            Type = "object",
            Properties = new Dictionary<string, OpenApiSchema>(),
            AdditionalPropertiesAllowed = true,
            AdditionalProperties = null
        };

        var result = target.FromOpenApi(schema, new ApiV1());

        result.IsDictionary.ShouldBeFalse();
    }

    [Fact]
    public void FromOpenApi_AdditionalPropertiesExplicitlyFalse_DoesNotSetAdditionalPropertiesSchema()
    {
        var autoMocker = new AutoMocker();

        autoMocker.GetMock<IDataTypeV1Transformer>()
            .Setup(x => x.FromOpenApi("object", It.IsAny<string>()))
            .Returns(DataTypeV1.Object);

        autoMocker.GetMock<ISchemaReferenceV1Transformer>()
            .Setup(x => x.FromOpenApi(It.IsAny<string>(), It.IsAny<SchemaDefinitionV1>(), It.IsAny<ApiV1>()))
            .Returns((string name, SchemaDefinitionV1 schema, ApiV1 _) =>
                new SchemaReferenceV1 { Name = name, Schema = schema, IsEmpty = false });

        var target = autoMocker.CreateInstance<SchemaDefinitionV1Transformer>();

        var schema = new OpenApiSchema
        {
            Type = "object",
            Properties = new Dictionary<string, OpenApiSchema> { ["id"] = new OpenApiSchema { Type = "string" } },
            AdditionalPropertiesAllowed = false,
            AdditionalProperties = null
        };

        var result = target.FromOpenApi(schema, new ApiV1());

        result.IsDictionary.ShouldBeFalse();
        result.AdditionalPropertiesSchema.IsEmpty.ShouldBeTrue();
    }
    private static SchemaDefinitionV1Transformer CreateRealTransformer() =>
        new(new EnumOptionV1Transformer(), new DataTypeV1Transformer(), new ExtensionV1Transformer(),
            new SchemaReferenceV1Transformer());

    private static OpenApiSchema CreateStatusEnumSchema() => new()
    {
        Type = "integer",
        Reference = new OpenApiReference { Id = "Status", Type = ReferenceType.Schema },
        Enum = new List<IOpenApiAny> { new OpenApiInteger(0), new OpenApiInteger(1) },
        Extensions = new Dictionary<string, IOpenApiExtension>
        {
            ["x-enumNames"] = new OpenApiArray { new OpenApiString("Active"), new OpenApiString("Inactive") }
        }
    };

    private static OpenApiSchema CreateWrapper(string keyword, bool nullable, params OpenApiSchema[] branches)
    {
        var wrapper = new OpenApiSchema { Nullable = nullable };
        var list = keyword switch
        {
            "oneOf" => wrapper.OneOf,
            "anyOf" => wrapper.AnyOf,
            "allOf" => wrapper.AllOf,
            _ => throw new ArgumentOutOfRangeException(nameof(keyword))
        };
        foreach (var branch in branches)
            list.Add(branch);
        return wrapper;
    }

    [Theory]
    [InlineData("oneOf", true)]
    [InlineData("anyOf", true)]
    [InlineData("allOf", true)]
    [InlineData("oneOf", false)]
    [InlineData("anyOf", false)]
    [InlineData("allOf", false)]
    public void FromOpenApi_SingleBranchComposition_IsMappedAsTheWrappedSchema(string keyword, bool nullable)
    {
        var target = CreateRealTransformer();

        var result = target.FromOpenApi(CreateWrapper(keyword, nullable, CreateStatusEnumSchema()), new ApiV1());

        result.IsEnum.ShouldBeTrue();
        result.Name.ShouldBe("Status");
        result.ReferenceId.ShouldBe("Status");
        result.DataType.ShouldBe(DataTypeV1.Int32);
        result.EnumOptions.Select(o => o.Name).ShouldBe(new[] { "Active", "Inactive" });
        result.Constraints.IsNullable.ShouldBe(nullable);
    }

    [Fact]
    public void FromOpenApi_SingleBranchCompositionWithNullTypeBranch_IsNullable()
    {
        var target = CreateRealTransformer();
        var wrapper = CreateWrapper("oneOf", false, CreateStatusEnumSchema(), new OpenApiSchema { Type = "null" });

        var result = target.FromOpenApi(wrapper, new ApiV1());

        result.IsEnum.ShouldBeTrue();
        result.Name.ShouldBe("Status");
        result.Constraints.IsNullable.ShouldBeTrue();
    }

    [Fact]
    public void FromOpenApi_SingleBranchCompositionWithNullEnumBranch_IsNullable()
    {
        var target = CreateRealTransformer();
        var nullBranch = new OpenApiSchema { Nullable = true, Enum = new List<IOpenApiAny> { new OpenApiNull() } };
        var wrapper = CreateWrapper("oneOf", false, nullBranch, CreateStatusEnumSchema());

        var result = target.FromOpenApi(wrapper, new ApiV1());

        result.IsEnum.ShouldBeTrue();
        result.Name.ShouldBe("Status");
        result.Constraints.IsNullable.ShouldBeTrue();
    }

    [Fact]
    public void FromOpenApi_EnumBranchWithNullAndOtherValues_IsNotTreatedAsNullBranch()
    {
        var target = CreateRealTransformer();
        var mixedBranch = new OpenApiSchema { Enum = new List<IOpenApiAny> { new OpenApiNull(), new OpenApiInteger(1) } };
        var wrapper = CreateWrapper("oneOf", false, mixedBranch, CreateStatusEnumSchema());

        var result = target.FromOpenApi(wrapper, new ApiV1());

        result.Name.ShouldBe("String");
        result.DataType.ShouldBe(DataTypeV1.String);
    }

    [Fact]
    public void FromOpenApi_SingleBranchComposition_WrapperDescriptionAndExtensionsWin()
    {
        var target = CreateRealTransformer();
        var inner = CreateStatusEnumSchema();
        inner.Description = "inner description";
        inner.Extensions["x-shared"] = new OpenApiString("inner");
        var wrapper = CreateWrapper("allOf", true, inner);
        wrapper.Description = "wrapper description";
        wrapper.Extensions["x-shared"] = new OpenApiString("wrapper");

        var result = target.FromOpenApi(wrapper, new ApiV1());

        result.Description.ShouldBe("wrapper description");
        result.ExtensionDictionary["x-shared"].ShouldBe("wrapper");
        result.ExtensionDictionary.ShouldContainKey("x-enumNames");
    }

    [Fact]
    public void FromOpenApi_ArrayOfSingleBranchComposition_IsMappedAsArrayOfTheWrappedSchema()
    {
        var target = CreateRealTransformer();
        var schema = new OpenApiSchema
        {
            Type = "array",
            Items = CreateWrapper("oneOf", true, CreateStatusEnumSchema())
        };

        var result = target.FromOpenApi(schema, new ApiV1());

        result.IsArray.ShouldBeTrue();
        result.IsEnum.ShouldBeTrue();
        result.Name.ShouldBe("Status");
        result.DataType.ShouldBe(DataTypeV1.Int32);
    }

    [Fact]
    public void FromOpenApi_SingleBranchCompositionOfObject_MapsObjectProperties()
    {
        var target = CreateRealTransformer();
        var inner = new OpenApiSchema
        {
            Type = "object",
            Reference = new OpenApiReference { Id = "Widget", Type = ReferenceType.Schema },
            Properties = new Dictionary<string, OpenApiSchema> { ["id"] = new OpenApiSchema { Type = "string" } }
        };

        var result = target.FromOpenApi(CreateWrapper("allOf", true, inner), new ApiV1());

        result.Name.ShouldBe("Widget");
        result.DataType.ShouldBe(DataTypeV1.Object);
        result.Properties.Select(p => p.OriginalName).ShouldBe(new[] { "id" });
        result.Constraints.IsNullable.ShouldBeTrue();
    }

    [Theory]
    [InlineData("oneOf")]
    [InlineData("anyOf")]
    [InlineData("allOf")]
    public void FromOpenApi_MultiBranchComposition_FallsBackToString(string keyword)
    {
        var target = CreateRealTransformer();
        var other = new OpenApiSchema
        {
            Type = "object",
            Reference = new OpenApiReference { Id = "Widget", Type = ReferenceType.Schema }
        };

        var result = target.FromOpenApi(CreateWrapper(keyword, false, CreateStatusEnumSchema(), other), new ApiV1());

        result.IsEnum.ShouldBeFalse();
        result.DataType.ShouldBe(DataTypeV1.String);
        result.Name.ShouldBe("String");
    }

    [Fact]
    public void FromOpenApi_MultipleCompositionKeywords_FallsBackToString()
    {
        var target = CreateRealTransformer();
        var wrapper = CreateWrapper("oneOf", false, CreateStatusEnumSchema());
        wrapper.AllOf.Add(CreateStatusEnumSchema());

        var result = target.FromOpenApi(wrapper, new ApiV1());

        result.IsEnum.ShouldBeFalse();
        result.DataType.ShouldBe(DataTypeV1.String);
    }

    [Fact]
    public void FromOpenApi_CompositionWithOwnProperties_IsNotUnwrapped()
    {
        var target = CreateRealTransformer();
        var wrapper = CreateWrapper("allOf", false, CreateStatusEnumSchema());
        wrapper.Type = "object";
        wrapper.Properties["extra"] = new OpenApiSchema { Type = "string" };

        var result = target.FromOpenApi(wrapper, new ApiV1());

        result.IsEnum.ShouldBeFalse();
        result.DataType.ShouldBe(DataTypeV1.Object);
        result.Properties.Select(p => p.OriginalName).ShouldBe(new[] { "extra" });
    }

    [Fact]
    public void FromOpenApi_ReferencedCompositionSchema_IsNotUnwrapped()
    {
        var target = CreateRealTransformer();
        var wrapper = CreateWrapper("oneOf", true, CreateStatusEnumSchema());
        wrapper.Reference = new OpenApiReference { Id = "NullableStatus", Type = ReferenceType.Schema };

        var result = target.FromOpenApi(wrapper, new ApiV1());

        result.Name.ShouldBe("NullableStatus");
        result.IsEnum.ShouldBeFalse();
    }

    [Fact]
    public void FromOpenApi_SelfReferenceThroughComposition_DoesNotStackOverflow()
    {
        var target = CreateRealTransformer();
        var node = new OpenApiSchema
        {
            Type = "object",
            Reference = new OpenApiReference { Id = "Node", Type = ReferenceType.Schema },
            Properties = new Dictionary<string, OpenApiSchema>()
        };
        node.Properties["parent"] = CreateWrapper("allOf", true, node);

        var result = target.FromOpenApi(node, new ApiV1());

        var parent = result.Properties.Single(p => p.OriginalName == "parent");
        parent.Schema.Name.ShouldBe("Node");
        parent.Schema.Constraints.IsNullable.ShouldBeTrue();
        parent.Schema.Properties.ShouldBeEmpty("circular ref should produce a stub with no nested properties");
    }
}
