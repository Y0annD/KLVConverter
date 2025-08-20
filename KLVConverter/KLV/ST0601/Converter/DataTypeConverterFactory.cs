namespace KLVConverter.KLV.ST0601.Converter;

public class DataTypeConverterFactory
{
    /// <summary>
    /// Get converter according to structure
    /// </summary>
    /// <param name="structure">Structure for which we want converter</param>
    /// <returns></returns>
    public static IConverter GetConverterForDataType(IConverterStructure structure)
    {
        ST0601Datatype inputType = structure.GetBinaryType();
        if (structure is ST0601ConverterStructure converterStructure)
        {
            switch (inputType)
            {
                case ST0601Datatype.STRING:
                    {
                        return new StringDataTypeConverter();
                    }
                case ST0601Datatype.INT8:
                case ST0601Datatype.UINT8:
                case ST0601Datatype.UINT16:
                case ST0601Datatype.UINT32:
                case ST0601Datatype.UINT64:
                case ST0601Datatype.INT16:
                case ST0601Datatype.INT32:
                case ST0601Datatype.INT64:
                    {
                        switch (structure.GetSoftwareType().Name)
                        {
                            case nameof(Single):
                            case nameof(Double):
                                {
                                    return new FloatDataTypeConverter(converterStructure.KLVType, structure.GetSoftwareType(), converterStructure.LSB, converterStructure.Offset);
                                }
                            case nameof(Byte):
                            case nameof(SByte):
                            case nameof(UInt16):
                            case nameof(UInt32):
                            case nameof(UInt64):
                            case nameof(Int16):
                            case nameof(Int32):
                            case nameof(Int64):
                            default:
                                {

                                    return new IntDataTypeConverter(converterStructure.KLVType, structure.GetSoftwareType(), converterStructure.LSB, converterStructure.Offset);
                                }

                        }
                    }
                case ST0601Datatype.IMAPB:
                    {

                        return new IMAPBDataTypeConverter(converterStructure.MinValue, converterStructure.MaxValue);
                    }
                case ST0601Datatype.FLOAT32:
                case ST0601Datatype.FLOAT64:
                    {
                        return new FloatDataTypeConverter(converterStructure.KLVType, structure.GetSoftwareType(), converterStructure.LSB, converterStructure.Offset);
                    }
                default:
                    {
                        // By default will be writen as string value
                        return new DefaultDatatypeConverter();
                    }
            }
        }
        else
        {
            return new DefaultDatatypeConverter();
        }

    }
}