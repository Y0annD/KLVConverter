using KLVConverter.KLV.ST0601;
using KLVConverter.KLV.ST0601.Converter;
using Microsoft.Extensions.Logging;

namespace KLVConverter.KLV;

public class ST0601Standard : ISMPTEImplementation
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
        ST0601Model.Add(22, new ST0601ConverterStructure("Target Width", ST0601Datatype.UINT16, ST0601Datatype.FLOAT32, 10000d / 65535d));
        ST0601Model.Add(23, new ST0601ConverterStructure("Frame Center Latitude", ST0601Datatype.FLOAT32, ST0601Datatype.INT32, 180d / 4294967294d));
        ST0601Model.Add(24, new ST0601ConverterStructure("Frame Center Longitude", ST0601Datatype.FLOAT32, ST0601Datatype.INT32, 360d / 4294967294d));
        ST0601Model.Add(25, new ST0601ConverterStructure("Frame Center Elevation", ST0601Datatype.FLOAT32, ST0601Datatype.UINT16, 19900d / 65535d, -900));
        ST0601Model.Add(26, new ST0601ConverterStructure("Offset Corner Latitude Point 1", ST0601Datatype.FLOAT32, ST0601Datatype.INT16, 0.15d/65534d));
        ST0601Model.Add(27, new ST0601ConverterStructure("Offset Corner Longitude Point 1", ST0601Datatype.FLOAT32, ST0601Datatype.INT16, 0.15d/65534d));
        ST0601Model.Add(28, new ST0601ConverterStructure("Offset Corner Latitude Point 2", ST0601Datatype.FLOAT32, ST0601Datatype.INT16, 0.15d/65534d));
        ST0601Model.Add(29, new ST0601ConverterStructure("Offset Corner Longitude Point 2", ST0601Datatype.FLOAT32, ST0601Datatype.INT16, 0.15d/65534d));
        ST0601Model.Add(30, new ST0601ConverterStructure("Offset Corner Latitude Point 3", ST0601Datatype.FLOAT32, ST0601Datatype.INT16, 0.15d/65534d));
        ST0601Model.Add(31, new ST0601ConverterStructure("Offset Corner Longitude Point 3", ST0601Datatype.FLOAT32, ST0601Datatype.INT16, 0.15d/65534d));
        ST0601Model.Add(32, new ST0601ConverterStructure("Offset Corner Latitude Point 4", ST0601Datatype.FLOAT32, ST0601Datatype.INT16, 0.15d/65534d));
        ST0601Model.Add(33, new ST0601ConverterStructure("Offset Corner Longitude Point 4", ST0601Datatype.FLOAT32, ST0601Datatype.INT16, 0.15d/65534d));
        ST0601Model.Add(34, new ST0601ConverterStructure("Icing Detected", ST0601Datatype.UINT8, ST0601Datatype.UINT8));
        ST0601Model.Add(35, new ST0601ConverterStructure("Wind direction", ST0601Datatype.FLOAT32, ST0601Datatype.UINT16, 360d / 65535d));
        ST0601Model.Add(36, new ST0601ConverterStructure("Wind speed", ST0601Datatype.FLOAT32, ST0601Datatype.UINT8, 100d / 255d));
        ST0601Model.Add(37, new ST0601ConverterStructure("Static Pressure", ST0601Datatype.FLOAT32, ST0601Datatype.UINT16, 5000d / 65535d));
        ST0601Model.Add(38, new ST0601ConverterStructure("Density altitude", ST0601Datatype.FLOAT32, ST0601Datatype.UINT16, 19900d / 65535d, -900));
        ST0601Model.Add(39, new ST0601ConverterStructure("Outside Air Temperature", ST0601Datatype.INT8, ST0601Datatype.INT8));
        ST0601Model.Add(40, new ST0601ConverterStructure("Target Location Latitude", ST0601Datatype.FLOAT64, ST0601Datatype.INT32, 180d / 4294967294d));
        ST0601Model.Add(41, new ST0601ConverterStructure("Target Location Longitude", ST0601Datatype.FLOAT64, ST0601Datatype.INT32, 360d / 4294967294d));
        ST0601Model.Add(42, new ST0601ConverterStructure("Target Location Elevation", ST0601Datatype.FLOAT32, ST0601Datatype.UINT16, 19900d / 65535d, -900));
        ST0601Model.Add(43, new ST0601ConverterStructure("Target Track Gate Width", ST0601Datatype.UINT16, ST0601Datatype.UINT8, 2d));
        ST0601Model.Add(44, new ST0601ConverterStructure("Target Track Gate Height", ST0601Datatype.UINT16, ST0601Datatype.UINT8, 2d));
        ST0601Model.Add(45, new ST0601ConverterStructure("Target Error Estimate-CE90", ST0601Datatype.FLOAT32, ST0601Datatype.UINT16, 4095d / 65535d));
        ST0601Model.Add(46, new ST0601ConverterStructure("Target Error Estimate-LE90", ST0601Datatype.FLOAT32, ST0601Datatype.UINT16, 4095d / 65535d));
        ST0601Model.Add(47, new ST0601ConverterStructure("Generic Flag Data", ST0601Datatype.UINT8, ST0601Datatype.UINT8));
        // TODO Item 48, Security Local Set
        ST0601Model.Add(49, new ST0601ConverterStructure("Differential Pressure", ST0601Datatype.FLOAT32, ST0601Datatype.UINT16, 5000d / 65535d));
        ST0601Model.Add(50, new ST0601ConverterStructure("Platform Angle of Attack", ST0601Datatype.FLOAT32, ST0601Datatype.INT16, 40d / 65534d));
        ST0601Model.Add(51, new ST0601ConverterStructure("Vertical Speed", ST0601Datatype.FLOAT32, ST0601Datatype.UINT16, 360d / 65534d));
        ST0601Model.Add(52, new ST0601ConverterStructure("Platform Sideslip angle", ST0601Datatype.FLOAT32, ST0601Datatype.INT16, 40d / 65534d));
        ST0601Model.Add(53, new ST0601ConverterStructure("Airfield Barometric pressure", ST0601Datatype.FLOAT32, ST0601Datatype.UINT16, 5000d / 65535d));
        ST0601Model.Add(54, new ST0601ConverterStructure("Airfield Elevation", ST0601Datatype.FLOAT32, ST0601Datatype.UINT16, 19900d / 65535d, -900));
        ST0601Model.Add(55, new ST0601ConverterStructure("Relative Humidity", ST0601Datatype.FLOAT32, ST0601Datatype.UINT8, 100d/255d));
        ST0601Model.Add(56, new ST0601ConverterStructure("Platform Ground Speed", ST0601Datatype.UINT8, ST0601Datatype.UINT8));
        ST0601Model.Add(57, new ST0601ConverterStructure("Ground Range", ST0601Datatype.FLOAT64, ST0601Datatype.UINT32, 5000000d / 4294967295d));
        ST0601Model.Add(58, new ST0601ConverterStructure("Fuel Remaining", ST0601Datatype.FLOAT32, ST0601Datatype.UINT16, 10000d / 65535d));
        ST0601Model.Add(59, new ST0601ConverterStructure("Platform Call Sign", ST0601Datatype.STRING, ST0601Datatype.STRING));
        ST0601Model.Add(60, new ST0601ConverterStructure("Weapon Load", ST0601Datatype.UINT16, ST0601Datatype.UINT16));
        ST0601Model.Add(61, new ST0601ConverterStructure("Weapon fired", ST0601Datatype.UINT8, ST0601Datatype.UINT8));
        ST0601Model.Add(62, new ST0601ConverterStructure("Laser PRF Code", ST0601Datatype.UINT16, ST0601Datatype.UINT16));
        ST0601Model.Add(63, new ST0601ConverterStructure("Sensor Field Of View name", ST0601Datatype.UINT8, ST0601Datatype.UINT8));
        ST0601Model.Add(64, new ST0601ConverterStructure("Platform Magnetic Heading", ST0601Datatype.FLOAT32, ST0601Datatype.UINT16, 360d / 65535d));
        ST0601Model.Add(65, new ST0601ConverterStructure("UAS DL Version", ST0601Datatype.UINT8, ST0601Datatype.UINT8));
        // Item 66 is deprecated
        ST0601Model.Add(67, new ST0601ConverterStructure("Alternate platform latitude", ST0601Datatype.FLOAT64, ST0601Datatype.INT32, 180d / 4294967294d));
        ST0601Model.Add(68, new ST0601ConverterStructure("Alternate platform longitude", ST0601Datatype.FLOAT64, ST0601Datatype.INT32, 360d / 4294967294d));
        ST0601Model.Add(69, new ST0601ConverterStructure("Alternate platform altitude", ST0601Datatype.FLOAT32, ST0601Datatype.UINT16, 19900d / 65535d, -900));
        ST0601Model.Add(70, new ST0601ConverterStructure("Alternate platform name", ST0601Datatype.STRING, ST0601Datatype.STRING));
        ST0601Model.Add(71, new ST0601ConverterStructure("Alternate platform heading", ST0601Datatype.FLOAT32, ST0601Datatype.UINT16, 360d / 65535d));
        ST0601Model.Add(72, new ST0601ConverterStructure("Event Start time", ST0601Datatype.UINT64, ST0601Datatype.UINT64));
        // TODO Item 73, RVT Local Set
        // TODO Item 74, VMTI Local Set
        ST0601Model.Add(75, new ST0601ConverterStructure("Sensor Ellipsoid Height", ST0601Datatype.FLOAT32, ST0601Datatype.UINT16, 19900d / 65535d, -900));
    ST0601Model.Add(76, new ST0601ConverterStructure("Alternate Platform Ellipsoid Height", ST0601Datatype.FLOAT32, ST0601Datatype.UINT16, 19900d / 65535d, -900));
        ST0601Model.Add(77, new ST0601ConverterStructure("Operational mode", ST0601Datatype.UINT8, ST0601Datatype.UINT8));
        ST0601Model.Add(78, new ST0601ConverterStructure("Frame Center Height Above Ellipsoid", ST0601Datatype.FLOAT32, ST0601Datatype.UINT16, 19900d / 65535d, -900));
        ST0601Model.Add(79, new ST0601ConverterStructure("Sensor North Velocity", ST0601Datatype.FLOAT32, ST0601Datatype.INT16, 654d / 65534d));
        ST0601Model.Add(80, new ST0601ConverterStructure("Sensor East Velocity", ST0601Datatype.FLOAT32, ST0601Datatype.INT16, 654d / 65534d));
        // TODO Item 81, Image horizon pixel pack
        ST0601Model.Add(82, new ST0601ConverterStructure("Corner Latitude Point 1 (Full)", ST0601Datatype.FLOAT64, ST0601Datatype.INT32, 180d / 4294967294d));
        ST0601Model.Add(83, new ST0601ConverterStructure("Corner Longitude Point 1 (Full)", ST0601Datatype.FLOAT64, ST0601Datatype.INT32, 360d / 4294967294d));
        ST0601Model.Add(84, new ST0601ConverterStructure("Corner Latitude Point 2 (Full)", ST0601Datatype.FLOAT64, ST0601Datatype.INT32, 180d / 4294967294d));
        ST0601Model.Add(85, new ST0601ConverterStructure("Corner Longitude Point 2 (Full)", ST0601Datatype.FLOAT64, ST0601Datatype.INT32, 360d / 4294967294d));
        ST0601Model.Add(86, new ST0601ConverterStructure("Corner Latitude Point 3 (Full)", ST0601Datatype.FLOAT64, ST0601Datatype.INT32, 180d / 4294967294d));
        ST0601Model.Add(87, new ST0601ConverterStructure("Corner Longitude Point 3 (Full)", ST0601Datatype.FLOAT64, ST0601Datatype.INT32, 360d / 4294967294d));
        ST0601Model.Add(88, new ST0601ConverterStructure("Corner Latitude Point 4 (Full)", ST0601Datatype.FLOAT64, ST0601Datatype.INT32, 180d / 4294967294d));
        ST0601Model.Add(89, new ST0601ConverterStructure("Corner Longitude Point 4 (Full)", ST0601Datatype.FLOAT64, ST0601Datatype.INT32, 360d / 4294967294d));
        ST0601Model.Add(90, new ST0601ConverterStructure("Platform Pitch Angle (Full)", ST0601Datatype.FLOAT64, ST0601Datatype.INT32, 180d / 4294967294d));
        ST0601Model.Add(91, new ST0601ConverterStructure("Platform Roll Angle (Full)", ST0601Datatype.FLOAT64, ST0601Datatype.INT32, 180d / 4294967294d));
        ST0601Model.Add(92, new ST0601ConverterStructure("Platform Angle of Attack (Full)", ST0601Datatype.FLOAT64, ST0601Datatype.INT32, 180d / 4294967294d));
        ST0601Model.Add(93, new ST0601ConverterStructure("Platform Sideslip Angle (Full)", ST0601Datatype.FLOAT64, ST0601Datatype.INT32, 360d / 4294967294d));
        // TODO Item 94, MIIS Core identifier
        // TODO Item 95, SAR Motion Imagery Local Set
        // TODO Item 96, Target Width Extended
        // TODO Item 97, Range Image Local Set
        // TODO Item 98, Geo-Registration Local Set
        // TODO Item 99, Composite Imaging Local Set
        // TODO Item 100, Segment Local Set
        // TODO Item 101, Amend Local Set
        // TODO Item 102, SDCC-FLP
        // TODO Item 103, Density Altitude Extended
        // TODO Item 104, Sensor Ellipsoid Height Extended
        // TODO Item 105, Alternate Platform Ellipsoid Height Extended
        ST0601Model.Add(106, new ST0601ConverterStructure("Stream Designator", ST0601Datatype.STRING, ST0601Datatype.STRING));
        ST0601Model.Add(107, new ST0601ConverterStructure("Operational Base", ST0601Datatype.STRING, ST0601Datatype.STRING));
        ST0601Model.Add(108, new ST0601ConverterStructure("Broadcast source", ST0601Datatype.STRING, ST0601Datatype.STRING));
        // TODO Item 109, Range to Recovery Location
        ST0601Model.Add(110, new ST0601ConverterStructure("Time Airborne", ST0601Datatype.UINT32, ST0601Datatype.UINT8));
        ST0601Model.Add(111, new ST0601ConverterStructure("Propulsion Unit Speed", ST0601Datatype.UINT32, ST0601Datatype.UINT8));
        // TODO Item 112, Platform Course Angle
        // TODO Item 113, Altitude AGL
        // TODO Item 114, Radar altimeter
        // TODO Item 115, Control Command
        // TODO Item 116, Control Command Verification List
        // TODO Item 117, Sensor Azimuth Rate
        // TODO Item 118, Sensor Elevation Rate
        // TODO Item 119, Sensor Roll Rate
        // TODO Item 120, On-Board MI Storage Percent Full
        // TODO Item 121, Active Wavelength List
        // TODO Item 122, Country codes
        ST0601Model.Add(123, new ST0601ConverterStructure("Number of Navsats in view", ST0601Datatype.UINT8, ST0601Datatype.UINT8));
        ST0601Model.Add(124, new ST0601ConverterStructure("Positioning method source", ST0601Datatype.UINT8, ST0601Datatype.UINT8));
        ST0601Model.Add(125, new ST0601ConverterStructure("Platform Status", ST0601Datatype.UINT8, ST0601Datatype.UINT8));
        ST0601Model.Add(126, new ST0601ConverterStructure("Sensor Control Mode", ST0601Datatype.UINT8, ST0601Datatype.UINT8));
        // TODO Item 127, Sensor Frame Rate Pack
        // TODO Item 128, Wavelength List
        ST0601Model.Add(129, new ST0601ConverterStructure("Target Id", ST0601Datatype.STRING, ST0601Datatype.STRING));
        // TODO Item 130, Airbase Locations
        ST0601Model.Add(131, new ST0601ConverterStructure("Take-off time", ST0601Datatype.UINT64, ST0601Datatype.UINT8));
        // TODO Item 132, Transmision Frequency
        ST0601Model.Add(133, new ST0601ConverterStructure("On-Board MI Storage Capacity", ST0601Datatype.UINT32, ST0601Datatype.UINT8));
        // TODO Item 134, zoom percentage
        ST0601Model.Add(135, new ST0601ConverterStructure("Communications method", ST0601Datatype.STRING, ST0601Datatype.STRING));
        ST0601Model.Add(136, new ST0601ConverterStructure("Leap Seconds", ST0601Datatype.INT32, ST0601Datatype.UINT8));
        ST0601Model.Add(137, new ST0601ConverterStructure("Correction Offset", ST0601Datatype.INT64, ST0601Datatype.INT8));
        // TODO Item 138, payload list
        // TODO Item 139, active payloads
        // TODO Item 140, weapons stores
        // TODO Item 141, Waypoint List
        // TODO Item 142, view domain
        // TODO Item 143, Metadata substream id pack
        

        // Init converter map
        foreach (KeyValuePair<int, ST0601ConverterStructure> entry in ST0601Model)
        {
            ConverterMap.Add(entry.Key, DataTypeConverterFactory.GetConverterForDataType(entry.Value));
        }
    }

    public byte[] GetDesignator()
    {
        return [0x2, 0xB, 0x1, 0x1, 0xE, 0x1, 0x3, 0x1, 0x1, 0x0, 0x0, 0x0];
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
    public string? ValueToStringOutput(KLVData input)
    {
        if (ConverterMap.ContainsKey(input.Key))
        {
            try
            {
                return ConverterMap.GetValueOrDefault(input.Key)?.Accept(input.Value);
            }
            catch (IOException e)
            {
                Logger.LogError(e, "Unable to convert value for Key: {key}, with length: {length}", input.Key, input.Length);
                return "";
            }
        }
        else
        {
            Logger.LogDebug("Tag {tag} not found in map", input.Key);
            return "[" + string.Join(",", input.Value) + "]";
        }
    }
}