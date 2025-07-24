namespace KLVConverter.KLV.ST0601.Converter;

public class DataTypeConverterFactory
{
    /// <summary>
    /// Get converter according to structure
    /// </summary>
    /// <param name="structure">Structure for which we want converter</param>
    /// <returns></returns>
    public static IConverter GetConverterForDataType(ST0601ConverterStructure structure)
    {
        switch (structure.Type)
        {
            case ST0601Datatype.STRING:
                {
                    return new StringDataTypeConverter();
                }
            case ST0601Datatype.UINT16:
            case ST0601Datatype.UINT64:
                {
                    return new IntDataTypeConverter(structure.KLVType, structure.Type, structure.LSB);
                }
            case ST0601Datatype.FLOAT32:
            case ST0601Datatype.FLOAT64:
                {
                    return new FloatDataTypeConverter(structure.KLVType, structure.Type, structure.LSB, structure.Offset);
                }
            default:
                {
                    // By default will be writen as string value
                    return new DefaultDatatypeConverter();

                }
        }

    }
}