using System.Collections;
using Microsoft.Extensions.Logging;

namespace KLVConverter.KLV;

/// <summary>
/// KLVManager constructor
/// </summary>
/// <param name="logger">Logger reference</param>
public class KLVManager(ILogger logger)
{
    /// <summary>
    /// Logger reference.
    /// </summary>
    private readonly ILogger Logger = logger;

    private readonly byte[] ST298_Ref = [0x6, 0xE, 0x2B, 0x34];

    /// <summary>
    /// Known implementations of specific standard
    /// </summary>
    public Dictionary<byte[], ISMPTEImplementation> Implementations = [];


    public byte[] SpecificULKeyFound(byte[] key)
    {
        foreach (byte[] implementation in Implementations.Keys)
        {
            if (StructuralComparisons.StructuralEqualityComparer.Equals(implementation, key))
            {
                return implementation;
            }
        }
        return [];
    }
    /// <summary>
    /// Seek stream to next valid message based on Reference UL Key.
    /// </summary>
    /// <param name="reader">reader that contains data</param>
    /// <returns></returns>
    public byte[] SeekToNextSMPTEKnownMessage(BinaryReader reader)
    {
        Logger.LogDebug("Seek to next message");
        byte[] ulKey = new byte[ST298_Ref.Length];
        byte[] specificKey = new byte[16 - ST298_Ref.Length];
        int nbRead;
        do
        {

            nbRead = reader.Read(ulKey);
            nbRead += reader.Read(specificKey);
            bool smpteFound = StructuralComparisons.StructuralEqualityComparer.Equals(ST298_Ref, ulKey);
            byte[] specificFound = SpecificULKeyFound(specificKey);
            if (smpteFound && specificFound.Length > 0)
            {
                Logger.LogDebug("Message found");
                return specificFound;
            }
            else
            {
                Logger.LogWarning("Key is not ST0601, try to recover better position");
                // Check if current key contains a subset of the start of the reference UL Key
                int checkLength = 1;
                int checkPosition = -1;
                byte[] compareUlArray = new byte[ST298_Ref.Length - 1];
                Array.Copy(ulKey, 1, compareUlArray, 0, ST298_Ref.Length - 1);
                byte[] checkArray;
                checkArray = new byte[checkLength];
                Array.Copy(ST298_Ref, checkArray, checkLength);
                bool isSubset = !checkArray.Except(compareUlArray).Any();
                if (isSubset)
                {
                    checkPosition = Array.IndexOf(compareUlArray, checkArray[0]);
                }
                if (checkPosition >= 0)
                {
                    reader.BaseStream.Seek(-16 + checkPosition + 1, SeekOrigin.Current);
                }
            }
        } while (nbRead > 0);
        Logger.LogInformation("No more message found");
        return [];
    }

    /// <summary>
    /// Read Basic Encoding Rule OID Length.
    /// If MSB of the first byte is one, it's an long encoded length.
    /// In this case, this byte means the number of bytes to read to retrieve length
    /// Else, this is the length
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    public int ReadOidLength(BinaryReader reader)
    {
        int value = 0;
        int read;
        read = reader.ReadByte();
        if ((read & 0x80) == 0x80)
        {
            // Long BER
            // nb of bytes to read 
            int nbBytesToRead = read & 0x7F;
            Logger.LogDebug("Ber Long form, length:{length}", nbBytesToRead);
            while (nbBytesToRead-- > 0)
            {
                value <<= 8;
                value += reader.ReadByte();
            }
        }
        else
        {
            Logger.LogDebug("Ber short form");
            value = read;
        }
        Logger.LogDebug("Length: {length}", value);
        return value;
    }

    /// <summary>
    /// Read the next available KLV Message.
    /// This method will automatically seek to next valid message.
    /// </summary>
    /// <param name="reader">Binary reader</param>
    /// <returns>List of available KLV data for this message. If empty, no more messages are available</returns>
    public SMPTEMessage? ReadNextKLVMessage(BinaryReader reader)
    {
        byte[] specificUlKey = SeekToNextSMPTEKnownMessage(reader);
        if (specificUlKey.Length > 0)
        {
            SMPTEMessage message = new(specificUlKey);
            // get length of data for the current message
            int length = ReadOidLength(reader);
            Logger.LogDebug("Length: {length}", length);
            byte[] value = new byte[length];
            reader.Read(value);
            Logger.LogDebug("Value: {value}", value);
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
                    Logger.LogWarning("Item length is higher than capacity for key {key} at stream position {position}. Remaining capacity: {remaining}, expected: {expected}", item.Key, reader.BaseStream.Position, value.Length - index, item.Length);
                    continue;
                }
                Array.Copy(value, index, item.Value, 0, item.Length);
                index += (int)item.Length;
                message.AddKLVData(item);
                Logger.LogDebug("KLV Data: {klv}", item.ToString());

            } while (index < value.Length);
            return message;
        }
        return null;
    }

    /// <summary>
    /// Register known SMPTE implementation standard.
    /// </summary>
    /// <param name="implementation">Implementation of SMPTE Standard</param>
    public void RegisterImplementation(ISMPTEImplementation implementation)
    {
        Implementations.Add(implementation.GetDesignator(), implementation);
    }
}