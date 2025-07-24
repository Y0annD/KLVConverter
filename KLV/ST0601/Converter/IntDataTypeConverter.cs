namespace KLVConverter.KLV.ST0601.Converter;

/// <summary>
/// Convert byte array to int value.
/// </summary>
/// <param name="input">input type</param>
/// <param name="output">output type</param>
/// <param name="lsb">lsb value</param>
public class IntDataTypeConverter(ST0601Datatype input, ST0601Datatype output, double lsb = 1) : IConverter
{

    private ST0601Datatype Input { get; set; } = input;
    private ST0601Datatype Output { get; set; } = output;
    private double LSB { get; set; } = lsb;
    public string Accept(byte[] data)
    {
        ulong value = 0;
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
            case ST0601Datatype.UINT64:
                {
                    value = BitConverter.ToUInt64(newData);
                    break;
                }
            default: break;
        }
        return Convert.ToString(value * LSB);
    }
}