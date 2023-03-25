using System;
using System.Linq;

namespace SnmpConverter;

internal static class EnumExtensions
{
    internal static SnmpResult<T> ToEnum<T>(this byte[] source, ref int offset) where T : Enum
    {
        var value = source[offset++];
        return Enum.GetValues(typeof(T)).Cast<byte>().Contains(value)
            ? new SnmpResult<T>((T)Convert.ChangeType(value, typeof(T)))
            : new SnmpResult<T>($"Incorrect value of {nameof(T)}");
    }

    internal static SnmpResult<T> ToEnum<T>(this int value) where T : Enum
    {
        return Enum.GetValues(typeof(T)).Cast<int>().Contains(value)
            ? new SnmpResult<T>((T)Convert.ChangeType(value, typeof(T)))
            : new SnmpResult<T>($"Incorrect value of {nameof(T)}");
    }
}