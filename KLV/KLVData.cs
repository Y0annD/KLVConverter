using System.Text;

namespace KLVConverter.KLV;

public class KLVData
{
    /// <summary>
    /// Data key.
    /// </summary>
    public byte Key { get; set; }
    /// <summary>
    /// Data Length.
    /// </summary>
    public long Length { get; set; }
    /// <summary>
    /// Data Value.
    /// </summary>
    public byte[] Value { get; set; } = [];

    public override string ToString()
    {
        return "K:" + Key + " L:" + Length + " V:[" + string.Join(",", Value) +"] - " +Encoding.UTF8.GetString(Value);
    }
}