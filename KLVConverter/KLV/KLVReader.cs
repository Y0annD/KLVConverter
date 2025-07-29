using System.Collections;
using Microsoft.Extensions.Logging;

namespace KLVConverter.KLV;

public class KLVReader
{
    /// <summary>
    /// Logger reference.
    /// </summary>
    private readonly ILogger Logger;

    /// <summary>
    /// KlvManager.
    /// </summary>
    private KLVManager KlvManager;

    /// <summary>
    /// KLV Reader constructor.
    /// </summary>
    /// <param name="logger">Reference logger</param>
    public KLVReader(ILogger logger)
    {
        Logger = logger;
        KlvManager = new(logger);
    }

    public List<List<KLVData>> ReadFile(string filePath)
    {
        List<List<KLVData>> data = [];
        using (var fs = new FileStream(@filePath, FileMode.Open))
        {
            Logger.LogInformation("{datafile}: {length} bytes", filePath, fs.Length);
            using BinaryReader binReader = new(fs);
            long position = fs.Position;
            while (KlvManager.SeekToNextMessage(binReader))
            {
                int length = 0;
                Logger.LogDebug("Key is ST0601 Key");
                length = KlvManager.ReadOidLength(binReader);
                Logger.LogDebug("Length: {length}", length);
                byte[] value = new byte[length];
                binReader.Read(value);
                Logger.LogDebug("Value: {value}", value);
                List<KLVData> localData = [];
                int index = 0;
                do
                {
                    KLVData item = new KLVData
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
                    localData.Add(item);
                    Logger.LogDebug("KLV Data: {klv}", item.ToString());

                } while (index < value.Length);
                data.Add(localData);
                Logger.LogDebug("Position: {key}", fs.Position);
            }
        }
        return data;
    }
}