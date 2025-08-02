using System.Collections;
using Microsoft.Extensions.Logging;

namespace KLVConverter.KLV;

/// <summary>
/// KLV Reader constructor.
/// </summary>
/// <param name="logger">Reference logger</param>
public class KLVReader
{
    /// <summary>
    /// Logger reference.
    /// </summary>
    private readonly ILogger Logger;

    /// <summary>
    /// KlvManager.
    /// </summary>
    private readonly KLVManager KlvManager;

    public KLVReader(ILogger logger)
    {
        Logger = logger;
        KlvManager = new(logger);
        KlvManager.RegisterImplementation(new ST0601Standard(logger));
    }

    /// <summary>
    /// Get the ordered list of data as Dictionnary.
    /// </summary>
    /// <param name="filePath">File to read</param>
    /// <returns>List of KLVData in this file</returns>
    public List<SMPTEMessage> ReadFile(string filePath)
    {
        List<SMPTEMessage> data = [];
        using (FileStream fs = new(@filePath, FileMode.Open))
        {
            Logger.LogInformation("{datafile}: {length} bytes", filePath, fs.Length);
            using BinaryReader binReader = new(fs);
            long position = fs.Position;
            SMPTEMessage? result;
            do
            {
                result = KlvManager.ReadNextKLVMessage(binReader);
                if (null != result)
                {
                    data.Add(result);
                }
            } while (result != null);
        }
        return data;
    }
}