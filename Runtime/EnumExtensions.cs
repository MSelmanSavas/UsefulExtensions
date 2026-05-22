using System;
using System.Collections.Generic;

namespace UsefulExtensions.Enum
{
    public static class EnumExtensions
    {
        public static IEnumerable<TEnum> Elements<TEnum>() where TEnum : struct, System.Enum
        {
            return (TEnum[])System.Enum.GetValues(typeof(TEnum));
        }

        public static int AsInt<TEnum>(this TEnum e) where TEnum : struct, System.Enum
        {
            return (int)(object)e;
        }

        public static T Next<T>(this T src) where T : struct, System.Enum
        {
            // Get all values of the enum as an array
            T[] arr = (T[])System.Enum.GetValues(src.GetType());

            // Find the index of the current value
            int j = System.Array.IndexOf(arr, src) + 1;

            // If we reached the end, wrap around to 0
            return (arr.Length == j) ? arr[0] : arr[j];
        }
    }
}

