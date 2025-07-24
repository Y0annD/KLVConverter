namespace KLVConverter.KLV.ST0601.Converter;

/// <summary>
/// Default data type converter that display all bytes as decimal value
/// </summary>
public class DefaultDatatypeConverter : IConverter
{
    public string Accept(byte[] data)
    {
        return "[" + string.Join(",", data) + "]";
    }
}