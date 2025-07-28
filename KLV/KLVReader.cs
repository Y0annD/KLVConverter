using System.Collections;
using Microsoft.Extensions.Logging;

namespace KLVConverter.KLV;

public class KLVReader
{
    private static readonly byte[] Ref_UL_KEY = [6, 0xE, 0x2B, 0x34, 0x2, 0xB, 0x1, 0x1, 0xE, 0x1, 0x3, 0x1, 0x1, 0x0, 0x0, 0x0];
    /// <summary>
    /// Logger reference.
    /// </summary>
    private readonly ILogger Logger;

    /// <summary>
    /// KLV Reader constructor.
    /// </summary>
    /// <param name="logger">Reference logger</param>
    public KLVReader(ILogger logger)
    {
        Logger = logger;
    }

    public List<List<KLVData>> ReadFile(string filePath)
    {
        List<List<KLVData>> data = [];
        using (var fs = new FileStream(@filePath, FileMode.Open))
        {
            Logger.LogInformation("{datafile}: {length} bytes", filePath, fs.Length);
            using BinaryReader binReader = new(fs);
            long position = fs.Position;
            do
            {
                byte[] ulKey = new byte[16];
                int length = 0;
                binReader.Read(ulKey);
                Logger.LogInformation("Key: {key}", ulKey);
                if (StructuralComparisons.StructuralEqualityComparer.Equals(Ref_UL_KEY, ulKey))
                {
                    Logger.LogInformation("Key is ST0601 Key");
                    length = ReadOidLength(binReader);
                    Logger.LogInformation("Length: {length}", length);
                    byte[] value = new byte[length];
                    binReader.Read(value);
                    Logger.LogInformation("Value: {value}", value);
                    List<KLVData> localData = [];
                    int index = 0;
                    do
                    {
                        KLVData item = new KLVData();
                        item.Key = value[index++];
                        item.Length = value[index++];
                        item.Value = new byte[item.Length];
                        Array.Copy(value, index, item.Value, 0, item.Length);
                        index += (int)item.Length;
                        localData.Add(item);
                        Logger.LogDebug("KLV Data: {klv}", item.ToString());

                    } while (index < value.Length);
                    data.Add(localData);
                    Logger.LogInformation("Position: {key}", fs.Position);
                }
                else
                {
                    Logger.LogWarning("Key is not ST0601");
                }
            } while (fs.Position < fs.Length);
        }
        return data;
    }

    /// <summary>
    /// Read Basic Encoding Rule OID Length.
    /// If MSB of the first byte is one, it's an long encoded length.
    /// In this case, this byte means the number of bytes to read to retrieve length
    /// Else, this is the length
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    private static int ReadOidLength(BinaryReader reader)
    {
        int value = 0;
        int read;
        read = reader.ReadByte();
        if ((read & 0x80) == 0x80)
        {
            // Long BER
            // nb of bytes to read 
            int nbBytesToRead = read & 0x7F;
            while (nbBytesToRead-- > 0)
            {
                value <<= 8;
                value += reader.ReadByte();
            }
        }
        else
        {
            value = read;
        }
        return value;
    }
}