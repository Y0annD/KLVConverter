namespace KLVConverter.KLV.ST0601.Converter;

/// <summary>
/// Convert byte array to string
/// </summary>
public class StringDataTypeConverter : IConverter
{
    public string Accept(byte[] data)
    {
        return System.Text.Encoding.UTF8.GetString(data);
    }
}