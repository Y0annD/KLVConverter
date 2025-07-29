namespace KLVConverter.KLV.ST0601.Converter;

/// <summary>
/// Convert byte array to float value
/// </summary>
/// <param name="input">input type</param>
/// <param name="output">output type</param>
/// <param name="lsb">value lsb</param>
/// <param name="offset">offset to apply to the output value</param>
public class FloatDataTypeConverter(ST0601Datatype input, ST0601Datatype output, double lsb, double offset = 0) : IConverter
{

    public ST0601Datatype Input { get; set; } = input;
    public ST0601Datatype Output { get; set; } = output;
    public double LSB { get; set; } = lsb;
    public double Offset { get; set; } = offset;
    public string Accept(byte[] data)
    {
        long value = 0;
        byte[] newData = data;
        Array.Reverse(newData);
        switch (Input)
        {
            case ST0601Datatype.UINT16:
                {
                    value = BitConverter.ToUInt16(newData);
                    break;
                }
            case ST0601Datatype.UINT32:
                {
                    value = BitConverter.ToUInt32(newData);
                    break;
                }
            case ST0601Datatype.INT16:
                {
                    value = BitConverter.ToInt16(newData);
                    break;
                }
            case ST0601Datatype.INT32:
                {
                    value = BitConverter.ToInt32(newData);
                    break;
                }
            default:
                break;
        }
        double result = LSB * value + Offset;
        return Convert.ToString(result);
    }
}