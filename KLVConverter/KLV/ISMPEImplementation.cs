namespace KLVConverter.KLV;

public interface ISMPTEImplementation
{

    /// <summary>
    /// Get implementation specific designator.
    /// </summary>
    /// <returns>Implementation SMPTE designator</returns>
    public byte[] GetDesignator();
    /// <summary>
    /// Get name associated to the tag.
    /// </summary>
    /// <param name="tag">tag value</param>
    /// <returns>tag name</returns>
    public String GetTagName(int tag);

    /// <summary>
    /// Convert binary value to string value.
    /// </summary>
    /// <param name="input">KLV raw value</param>
    /// <returns>String value of the data</returns>
    public string? ValueToStringOutput(KLVData input);
}