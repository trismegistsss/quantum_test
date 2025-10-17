using UnityEngine;

namespace  Utils
{
    public class HexColorUtils
    {
        private static int _stringOffset = 0;

        public static Color HexToColor(string hex)
        {
            byte r = byte.MinValue;
            byte g = byte.MinValue;
            byte b = byte.MinValue;
            byte a = byte.MaxValue;

            if (!string.IsNullOrEmpty(hex))
            {
                //Check if HEX was set with a # symbol
                if (hex.StartsWith("#", System.StringComparison.CurrentCulture))
                {
                    _stringOffset++;
                }

                r = byte.Parse(hex.Substring(0 + _stringOffset, 2), System.Globalization.NumberStyles.HexNumber);
                g = byte.Parse(hex.Substring(2 + _stringOffset, 2), System.Globalization.NumberStyles.HexNumber);
                b = byte.Parse(hex.Substring(4 + _stringOffset, 2), System.Globalization.NumberStyles.HexNumber);

                if (hex.Length > 7)
                {
                    a = byte.Parse(hex.Substring(6 + _stringOffset, 2), System.Globalization.NumberStyles.HexNumber);
                }
            }

            return new Color32(r, g, b, a);
        }

        public static string ColorToHex(Color c)
        {
            return string.Format("#{0:X2}{1:X2}{2:X2}{3:x2}", ToByte(c.r), ToByte(c.g), ToByte(c.b), ToByte(c.a));
        }

        private static byte ToByte(float f)
        {
            f = Mathf.Clamp01(f);
            return (byte)(f * 255);
        }
    }
}