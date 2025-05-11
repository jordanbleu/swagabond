using System.ComponentModel.DataAnnotations;

namespace Swagabond.ObjectModelV1.Transformer;

public interface IDataTypeV1Transformer
{
    DataTypeV1 FromOpenApi(string? dataType, string? format);
}

public class DataTypeV1Transformer : IDataTypeV1Transformer
{
    public DataTypeV1 FromOpenApi(string? dataType, string? format)
    {
        if (dataType == null) 
        {
            return DataTypeV1.String;
        }
        
        if (dataType.Equals("string", StringComparison.OrdinalIgnoreCase))
        {
            if (format == "date-time")
            {
                return DataTypeV1.DateTime;
            } 
            
            if (format == "uuid")
            {
                return DataTypeV1.Guid;
            }

            return DataTypeV1.String;
        }
        
        if (dataType.Equals("integer", StringComparison.OrdinalIgnoreCase))
        {
            if (format == "int32")
            {
                return DataTypeV1.Int32;
            }
            
            if (format == "int64")
            {
                return DataTypeV1.Int64;
            }

            return DataTypeV1.Int32;
        }
        
        if (dataType.Equals("number", StringComparison.OrdinalIgnoreCase))
        {
            if (format == "float")
            {
                return DataTypeV1.Float;
            }
            
            if (format == "double")
            {
                return DataTypeV1.Double;
            }
            
            if (format == "decimal")
            {
                return DataTypeV1.Decimal;
            }
            
            return DataTypeV1.Double;
        }
        
        if (dataType.Equals("boolean", StringComparison.OrdinalIgnoreCase))
        {
            return DataTypeV1.Boolean;
        }
        
        if (dataType.Equals("object", StringComparison.OrdinalIgnoreCase))
        {
            return DataTypeV1.Object;
        }

        return DataTypeV1.String;
    }
}