using System.ComponentModel.DataAnnotations;

namespace KLVConverter.KLV.ST0601;


public class ST0601ConverterStructure(string name, ST0601Datatype type, ST0601Datatype klvType, double lsb = 1, double offset = 0, double min=0, double max=0)
{
    /// <summary>
    /// Name of the structure
    /// </summary>
    public string Name { get; set; } = name;
    /// <summary>
    /// Software type of the structure
    /// </summary>
    public ST0601Datatype Type { get; set; } = type;
    /// <summary>
    /// KLV input type
    /// </summary>
    public ST0601Datatype KLVType { get; set; } = klvType;

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
}