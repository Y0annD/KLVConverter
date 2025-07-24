namespace KLVConverter.KLV.ST0601.Converter;

public interface IConverter
{
    /// <summary>
    /// Accept a value and convert it to string value.
    /// </summary>
    /// <param name="data">value to convert to string</param>
    /// <returns>string result</returns>
    public string Accept(byte[] data);
}