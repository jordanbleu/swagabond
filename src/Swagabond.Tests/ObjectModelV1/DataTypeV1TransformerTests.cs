using Shouldly;
using Swagabond.ObjectModelV1;
using Swagabond.ObjectModelV1.Transformer;

namespace Swagabond.Tests.ObjectModelV1;

public class DataTypeV1TransformerTests
{
    private readonly DataTypeV1Transformer _target = new();

    [Theory]
    [InlineData(null, null, DataTypeV1.String)]
    [InlineData("string", null, DataTypeV1.String)]
    [InlineData("string", "date-time", DataTypeV1.DateTime)]
    [InlineData("string", "uuid", DataTypeV1.Guid)]
    [InlineData("string", "unknown-format", DataTypeV1.String)]
    [InlineData("integer", null, DataTypeV1.Int32)]
    [InlineData("integer", "int32", DataTypeV1.Int32)]
    [InlineData("integer", "int64", DataTypeV1.Int64)]
    [InlineData("number", null, DataTypeV1.Double)]
    [InlineData("number", "float", DataTypeV1.Float)]
    [InlineData("number", "double", DataTypeV1.Double)]
    [InlineData("number", "decimal", DataTypeV1.Decimal)]
    [InlineData("boolean", null, DataTypeV1.Boolean)]
    [InlineData("object", null, DataTypeV1.Object)]
    public void FromOpenApi_ReturnsExpectedDataType(string? dataType, string? format, DataTypeV1 expected)
    {
        _target.FromOpenApi(dataType, format).ShouldBe(expected);
    }

    [Theory]
    [InlineData("Integer", DataTypeV1.Int32)]
    [InlineData("INTEGER", DataTypeV1.Int32)]
    [InlineData("Number", DataTypeV1.Double)]
    [InlineData("NUMBER", DataTypeV1.Double)]
    [InlineData("Boolean", DataTypeV1.Boolean)]
    [InlineData("BOOLEAN", DataTypeV1.Boolean)]
    [InlineData("Object", DataTypeV1.Object)]
    [InlineData("OBJECT", DataTypeV1.Object)]
    [InlineData("String", DataTypeV1.String)]
    [InlineData("STRING", DataTypeV1.String)]
    public void FromOpenApi_IsCaseInsensitive(string dataType, DataTypeV1 expected)
    {
        _target.FromOpenApi(dataType, null).ShouldBe(expected);
    }

    [Theory]
    [InlineData("foobar")]
    [InlineData("array")]
    [InlineData("")]
    public void FromOpenApi_UnknownType_DefaultsToString(string dataType)
    {
        _target.FromOpenApi(dataType, null).ShouldBe(DataTypeV1.String);
    }
}
