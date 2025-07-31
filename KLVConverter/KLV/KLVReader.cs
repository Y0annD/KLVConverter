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
            while (KlvManager.SeekToNextMessage(binReader))
            {
                Logger.LogDebug("Key is ST0601 Key");
                int length = KlvManager.ReadOidLength(binReader);
                Logger.LogDebug("Length: {length}", length);
                byte[] value = new byte[length];
                binReader.Read(value);
                Logger.LogDebug("Value: {value}", value);
                Dictionary<int, KLVData> localData = [];
                int index = 0;
                do
                {
                    KLVData item = new()
                    {
                        Key = value[index++],
                        Length = value[index++]
                    };
                    item.Value = new byte[item.Length];
                    // Check that remaining length is sufficient to contains this tag value
                    if (index + item.Length > value.Length)
                    {
                        // Item length is higher than remaining bytes to read
                        Logger.LogWarning("Item length is higher than capacity for key {key} at stream position {position}. Remaining capacity: {remaining}, expected: {expected}", item.Key, fs.Position, value.Length - index, item.Length);
                        continue;
                    }
                    Array.Copy(value, index, item.Value, 0, item.Length);
                    index += (int)item.Length;
                    localData.Add(item.Key, item);
                    Logger.LogDebug("KLV Data: {klv}", item.ToString());

                } while (index < value.Length);
                data.Add(localData);
                Logger.LogDebug("Position: {key}", fs.Position);
            }
        }
        return data;
    }
}