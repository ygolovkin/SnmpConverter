using System;
using System.Linq;

namespace SnmpConverter;

internal static class EnumExtensions
{
    internal static T ToEnum<T>(this byte[] source, ref int offset) where T : Enum
    {
        var value = source[offset++];
        if (!Enum.GetValues(typeof(T)).Cast<byte>().Contains(value))
        {
            throw new SnmpException($"Incorrect value of {nameof(T)}.");
        }

        return (T)Convert.ChangeType(value, typeof(T));
    }

    internal static T ToEnum<T>(this int value) where T : Enum
    {
        if(!Enum.GetValues(typeof(T)).Cast<int>().Contains(value))
        {
            throw new SnmpException($"Incorrect value of {nameof(T)}");
        }

        return (T)Convert.ChangeType(value, typeof(T));
    }
}