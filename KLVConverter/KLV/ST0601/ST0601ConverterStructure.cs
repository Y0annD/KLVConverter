
namespace KLVConverter.KLV.ST0601;


public class ST0601ConverterStructure(string name,  Type softwareType, ST0601Datatype type, double lsb = 1, double offset = 0, double min=0, double max=0): IConverterStructure
{
    /// <summary>
    /// Name of the structure
    /// </summary>
    public string Name { get; set; } = name;
    /// <summary>
    /// Software type of the structure
    /// </summary>
    public Type SoftwareType { get;  set; } = softwareType;
    /// <summary>
    /// KLV input type
    /// </summary>
    public ST0601Datatype KLVType { get;  set; } = type;

    /// <summary>
    /// LSB to apply for a KLV input
    /// </summary>
    public double LSB { get; set; } = lsb;
    /// <summary>
    /// Offset to apply to a software type after conversion
    /// </summary>
    public double Offset { get; set; } = offset;
    /// <summary>
    /// Minimum value for the attribute
    /// </summary>
    public double MinValue { get; set; } = min;
    /// <summary>
    /// Maximum value for the attribute
    /// </summary>
    public double MaxValue { get; set; } = max;

    public ST0601Datatype GetBinaryType()
    {
        return KLVType;
    }

    public Type GetSoftwareType()
    {
        return SoftwareType;
    }
}