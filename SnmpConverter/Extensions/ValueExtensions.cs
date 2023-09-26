using System;

namespace SnmpConverter;

internal static class ValueExtensions
{
    internal static byte[] ToValue(this byte[] source, ref int offset)
    {
        var length = source.ToLength(ref offset, x => x < 0, "Incorrect value's length.");

        var value = new byte[length];
        Buffer.BlockCopy(source, offset, value, 0, length);
        offset += length;

        return value;
    }

    internal static byte[] ToValueArray(this byte[]? value)
    {
        value ??= Array.Empty<byte>();
        return value.ToArrayWithLength();
    }
}