using System.Globalization;
using UnityEngine;

namespace UsefulExtensions.Float
{
    public static class FloatExtensions
    {
        /// <summary>
        /// Takes an angle (Z) in degrees an converts it to a 2D unit vector.
        /// </summary>
        /// <param name="angle">Angle in degrees.</param>
        /// <returns></returns>
        public static UnityEngine.Vector2 AngleToDirection(this float angle)
        {
            var angleInRadians = Mathf.Deg2Rad * angle;
            var x = Mathf.Cos(angleInRadians);
            var y = Mathf.Sin(angleInRadians);

            return new UnityEngine.Vector2(x, y);
        }

        public static float WithEpsilon(this float value) => value + Mathf.Epsilon;

        public static string CustomDecimalSeparator(this float value, string separator)
        {
            NumberFormatInfo numberFormatInfo = new NumberFormatInfo();
            numberFormatInfo.NumberDecimalSeparator = separator;

            return value.ToString(numberFormatInfo);
        }
        
        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
    }

}
