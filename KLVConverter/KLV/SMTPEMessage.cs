namespace KLVConverter.KLV;

public class SMPTEMessage(byte[] identifier)
{
    /// <summary>
    /// KLV datas for this message.
    /// </summary>
    private readonly Dictionary<int, KLVData> KlvDatas = [];
    /// <summary>
    /// List of warinings for this message.
    /// </summary>
    private readonly List<string> Warnings = [];

    /// <summary>
    /// SMPTE identifier of the message
    /// </summary>
    public byte[] Identifier { get; } = identifier;


    /// <summary>
    /// Add a new KLV Data to this message
    /// </summary>
    /// <param name="klv"></param>
    public void AddKLVData(KLVData klv)
    {
        try
        {
            KlvDatas.Add(klv.Key, klv);
        }
        catch (ArgumentException)
        {
            // Key already exist
            Warnings.Add("Key " + klv.Key + " already exist for this message");
        }
    }

    /// <summary>
    /// Does this message has warnings
    /// </summary>
    /// <returns>True if this message contains warnings</returns>
    public bool HasWarnings()
    {
        return Warnings.Count > 0;
    }

    /// <summary>
    /// Retrieve this message warnings.
    /// </summary>
    /// <returns>message warnings</returns>
    public List<string> GetWarnings()
    {
        return Warnings;
    }

    /// <summary>
    /// Return the KLV datas.
    /// </summary>
    /// <returns>Klv datas</returns>
    public Dictionary<int, KLVData> GetDatas()
    {
        return KlvDatas;
    }
}