namespace KLVConverter.KLV.ST0601.Converter;

/// <summary>
/// Convert IMAP attribute to integer
/// </summary>
public class IMAPAConverter (int min, int max): IConverter
{

    private int Min { get; set; } = min;
    private int Max { get; set; } = max;
    public string Accept(byte[] data)
    {
        // if length is known

        // if precisin is known
        
        throw new NotImplementedException();
    }
}