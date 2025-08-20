namespace KLVConverter.KLV.ST0601;

/// <summary>
/// Interface for converter structure.
/// </summary>
public interface IConverterStructure
{
    /// <summary>
    /// Type of binary data in ST0601 Standard.
    /// </summary>
    /// <returns>Binary data type</returns>
    public ST0601Datatype GetBinaryType();

    /// <summary>
    /// Get Software type of data.
    /// </summary>
    /// <returns>Software type</returns>
    public Type GetSoftwareType();
}