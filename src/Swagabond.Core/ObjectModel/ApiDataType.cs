namespace Swagabond.Core.ObjectModel;

public enum ApiDataType
{
    String,
    Number,
    Integer,
    Boolean,
    Array,
    Object
}

public static class ApiDataTypeMapper
{
    public static ApiDataType FromString(string t)
    {
        return t.ToLower() switch
        {
            "string" => ApiDataType.String,
            "number" => ApiDataType.Number,
            "integer" => ApiDataType.Integer,
            "boolean" => ApiDataType.Boolean,
            "array" => ApiDataType.Array,
            "object" => ApiDataType.Object,
            _ => ApiDataType.String
        };
    }
}