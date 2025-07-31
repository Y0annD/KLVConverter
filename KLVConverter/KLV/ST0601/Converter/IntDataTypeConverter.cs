namespace KLVConverter.KLV.ST0601.Converter;

/// <summary>
/// Convert byte array to int value.
/// </summary>
/// <param name="input">input type</param>
/// <param name="output">output type</param>
/// <param name="lsb">lsb value</param>
public class IntDataTypeConverter(ST0601Datatype input, ST0601Datatype output, double lsb = 1, int offset = 0) : IConverter
{
    /// <summary>
    /// Input data type.
    /// </summary>
    private ST0601Datatype Input { get; set; } = input;
    /// <summary>
    /// Output data type.
    /// </summary>
    private ST0601Datatype Output { get; set; } = output;
    /// <summary>
    /// Less Significant Byte value.
    /// </summary>
    private double LSB { get; set; } = lsb;

    /// <summary>
    /// Offset to output data value
    /// </summary>
    private int Offset { get; set; } = offset;

    public string Accept(byte[] data)
    {
        long value = 0;
        byte[] newData = data;
        Array.Reverse(newData);
        switch (Input)
        {
            case ST0601Datatype.UINT8:
                {
                    CheckArrayLength(newData, 1);
                    value = newData[0];
                    break;
                }
            case ST0601Datatype.UINT16:
                {
                    CheckArrayLength(newData, 2);
                    value = BitConverter.ToUInt16(newData);
                    break;
                }
            case ST0601Datatype.INT16:
                {
                    CheckArrayLength(newData, 2);
                    value = BitConverter.ToInt16(newData);
                    break;
                }
            case ST0601Datatype.INT32:
                {
                    CheckArrayLength(newData, 4);
                    value = BitConverter.ToInt32(newData);
                    break;
                }
            case ST0601Datatype.UINT32:
                {
                    CheckArrayLength(newData, 4);
                    value = BitConverter.ToUInt32(newData);
                    break;
                }
            case ST0601Datatype.UINT64:
                {
                    CheckArrayLength(newData, 8);
                    return Convert.ToString(BitConverter.ToUInt64(newData) + (ulong)Offset);

                }
            default: break;
        }
        return Convert.ToString(value * LSB + Offset);
    }

    /// <summary>
    /// Check if the input data array is longer enough for future conversion.
    /// </summary>
    /// <param name="data">data to check</param>
    /// <param name="expectedLength">Expected array length</param>
    /// <exception cref="IOException">Exception if data is not longer enough</exception>
    private static void CheckArrayLength(byte[] data, int expectedLength)
    {
        if (data.Length < expectedLength)
        {
            throw new IOException("Insufficient array length. Expected " + expectedLength + " but was " + data.Length);
        }
    }
}