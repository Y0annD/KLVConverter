using KLVConverter.KLV.ST0601;
using KLVConverter.KLV.ST0601.Converter;
using Microsoft.Extensions.Logging;

namespace KLVConverter.KLV;

public class ST0601Standard
{
    /// <summary>
    /// Reference of logger.
    /// </summary>
    ILogger Logger;

    /// <summary>
    /// Model that contains attribute description
    /// </summary>
    private Dictionary<int, ST0601ConverterStructure> ST0601Model = [];

    private Dictionary<int, IConverter> ConverterMap = [];

    public ST0601Standard(ILogger logger)
    {
        Logger = logger;
        ST0601Model.Add(1, new ST0601ConverterStructure("Checksum", ST0601Datatype.UINT16, ST0601Datatype.UINT16));
        ST0601Model.Add(2, new ST0601ConverterStructure("Timestamp", ST0601Datatype.UINT64, ST0601Datatype.UINT64));
        ST0601Model.Add(3, new ST0601ConverterStructure("Mission ID", ST0601Datatype.STRING, ST0601Datatype.STRING));
        ST0601Model.Add(4, new ST0601ConverterStructure("Platform Tail Number", ST0601Datatype.STRING, ST0601Datatype.STRING));
        ST0601Model.Add(5, new ST0601ConverterStructure("Platform Heading Angle", ST0601Datatype.FLOAT64, ST0601Datatype.UINT16, 360f / 65535f));
        ST0601Model.Add(6, new ST0601ConverterStructure("Platform Pitch Angle", ST0601Datatype.FLOAT32, ST0601Datatype.INT16, 40f / 65534f));
        ST0601Model.Add(7, new ST0601ConverterStructure("Platform Roll Angle", ST0601Datatype.FLOAT32, ST0601Datatype.INT16, 100f / 65534f));
        ST0601Model.Add(8, new ST0601ConverterStructure("Platform True Air Speed", ST0601Datatype.UINT8, ST0601Datatype.UINT8));
        ST0601Model.Add(9, new ST0601ConverterStructure("Indicated Air Speed", ST0601Datatype.UINT8, ST0601Datatype.UINT8));
        ST0601Model.Add(10, new ST0601ConverterStructure("Platform Designation", ST0601Datatype.STRING, ST0601Datatype.STRING));
        ST0601Model.Add(11, new ST0601ConverterStructure("Image Source Sensor", ST0601Datatype.STRING, ST0601Datatype.STRING));
        ST0601Model.Add(12, new ST0601ConverterStructure("Image Coordinate System", ST0601Datatype.STRING, ST0601Datatype.STRING));
        ST0601Model.Add(13, new ST0601ConverterStructure("Platform Latitude", ST0601Datatype.FLOAT64, ST0601Datatype.INT32, 180d / 4294967294d));
        ST0601Model.Add(14, new ST0601ConverterStructure("Platform Longitude", ST0601Datatype.FLOAT64, ST0601Datatype.INT32, 360d / 4294967294d));
        ST0601Model.Add(15, new ST0601ConverterStructure("Sensor True Altitude", ST0601Datatype.FLOAT32, ST0601Datatype.UINT16, 19900d / 65535d, -900d));
        ST0601Model.Add(16, new ST0601ConverterStructure("Sensor Horizontal Field Of View", ST0601Datatype.FLOAT32, ST0601Datatype.UINT16, 180d / 65535d));
        ST0601Model.Add(17, new ST0601ConverterStructure("Sensor Vertical Field of View", ST0601Datatype.FLOAT32, ST0601Datatype.UINT16, 180d / 65535d));
        ST0601Model.Add(18, new ST0601ConverterStructure("Sensor Relative Azimuth angle", ST0601Datatype.FLOAT64, ST0601Datatype.UINT32, 360d / 4294967295d));
        ST0601Model.Add(19, new ST0601ConverterStructure("Sensor Relative Elevation angle", ST0601Datatype.FLOAT64, ST0601Datatype.INT32, 360d / 4294967294d));
        ST0601Model.Add(20, new ST0601ConverterStructure("Sensor Relative Roll angle", ST0601Datatype.FLOAT64, ST0601Datatype.UINT32, 360d / 4294967294d));
        ST0601Model.Add(21, new ST0601ConverterStructure("Slant Range", ST0601Datatype.FLOAT64, ST0601Datatype.UINT32, 5000000d / 4294967295d));
        ST0601Model.Add(59, new ST0601ConverterStructure("Platform Call Sign", ST0601Datatype.STRING, ST0601Datatype.STRING));
        ST0601Model.Add(65, new ST0601ConverterStructure("UAS DL Version", ST0601Datatype.UINT8, ST0601Datatype.UINT8));
        ST0601Model.Add(70, new ST0601ConverterStructure("Alternate platform name", ST0601Datatype.STRING, ST0601Datatype.STRING));
        ST0601Model.Add(106, new ST0601ConverterStructure("Stream Designator", ST0601Datatype.STRING, ST0601Datatype.STRING));
        ST0601Model.Add(107, new ST0601ConverterStructure("Operational Base", ST0601Datatype.STRING, ST0601Datatype.STRING));
        ST0601Model.Add(108, new ST0601ConverterStructure("Broadcast source", ST0601Datatype.STRING, ST0601Datatype.STRING));

        // Init converter map
        foreach (KeyValuePair<int, ST0601ConverterStructure> entry in ST0601Model)
        {
            ConverterMap.Add(entry.Key, DataTypeConverterFactory.GetConverterForDataType(entry.Value));
        }
    }

    /// <summary>
    /// Get name to display if exist for the specified tag.
    /// </summary>
    /// <param name="tag">Tag to get name</param>
    /// <returns>Tag value + Tag name</returns>
    public string GetTagName(int tag)
    {
        if (ST0601Model.ContainsKey(tag))
        {
            ST0601ConverterStructure? structure = ST0601Model.GetValueOrDefault(tag);
            if (structure != null)
            {
                return tag + ":" + structure.Name;
            }
            else
            {
                return Convert.ToString(tag);
            }
        }
        else
        {
            return Convert.ToString(tag);
        }
    }

    /// <summary>
    /// Convert KLV value to string value according ST0601 definition.
    /// </summary>
    /// <param name="input">input to convert</param>
    /// <returns>value as string</returns>
    public string? KlvToStringOutput(KLVData input)
    {
        if (ConverterMap.ContainsKey(input.Key))
        {
            return ConverterMap.GetValueOrDefault(input.Key)?.Accept(input.Value);
        }
        else
        {
            Logger.LogDebug("Tag {tag} not found in map", input.Key);
            return "[" + string.Join(",", input.Value) + "]";
        }
    }
}