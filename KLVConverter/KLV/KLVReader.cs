using System.Collections;
using Microsoft.Extensions.Logging;

namespace KLVConverter.KLV;

/// <summary>
/// KLV Reader constructor.
/// </summary>
/// <param name="logger">Reference logger</param>
public class KLVReader(ILogger logger)
{
    /// <summary>
    /// Logger reference.
    /// </summary>
    private readonly ILogger Logger = logger;

    /// <summary>
    /// KlvManager.
    /// </summary>
    private readonly KLVManager KlvManager = new(logger);

    /// <summary>
    /// Get the ordered list of data as Dictionnary.
    /// </summary>
    /// <param name="filePath">File to read</param>
    /// <returns>List of KLVData in this file</returns>
    public List<Dictionary<int, KLVData>> ReadFile(string filePath)
    {
        List<Dictionary<int, KLVData>> data = [];
        using (FileStream fs = new(@filePath, FileMode.Open))
        {
            Logger.LogInformation("{datafile}: {length} bytes", filePath, fs.Length);
            using BinaryReader binReader = new(fs);
            long position = fs.Position;
            Dictionary<int, KLVData> result;
            do
            {
                result = KlvManager.ReadNextKLVMessage(binReader);
                if (result.Count > 0)
                {
                    data.Add(result);
                }
            } while (result.Count > 0);
        }
        return data;
    }
}