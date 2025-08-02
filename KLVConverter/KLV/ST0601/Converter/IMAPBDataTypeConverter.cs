using System.Runtime.Intrinsics.Arm;

namespace KLVConverter.KLV.ST0601.Converter;

/// <summary>
/// Convert IMAP attribute to integer
/// </summary>
public class IMAPBDataTypeConverter(double min, double max) : IConverter
{

    private double Min { get; set; } = min;
    private double Max { get; set; } = max;
    public string Accept(byte[] data)
    {
        // Use reverse mapping

        double y = 0;
        bool special = ((data[0] >> 7) & data[0] >> 6 & 1) == 1;
        if (special)
        {
            return ((data[0] >> 3) & 7) switch
            {
                0 => "User defined",
                1 => "Infinity",
                5 => "-Infinity",
                2 or 3 or 6 or 7 => "NaN",
                4 => "MISB Special Value",
                _ => "",
            };
        }
        else
        {
            if (data.Length == 2)
            {
                Array.Reverse(data);
                y = BitConverter.ToUInt16(data);

            }
            else if (data.Length == 3)
            {
                byte[] d = [data[2], data[1], data[0], 0];
                y = BitConverter.ToUInt32(d);
            }
            else if (data.Length == 4)
            {
                Array.Reverse(data);
                y = BitConverter.ToUInt32(data);
            }
            double bPow = Math.Ceiling(Math.Log2(Max - Min));
            double dPow = 8 * data.Length - 1;
            double sf = Math.Pow(2, dPow - bPow);
            // Resolution
            double sr = Math.Pow(2, bPow - dPow);
            double zOffset = 0.0;
            if (Min < 0 && Max > 0)
            {
                zOffset = sf * Min - Math.Floor(sf * Min);
            }
            // double y = Math.Truncate(sf * (x - Min) + zOffset);
            // double y = BitConverter.ToUInt16(data);
            double x = sr * (y - zOffset) + Min;
            return Convert.ToString(x);
        }
    }
}