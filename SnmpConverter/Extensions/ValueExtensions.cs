using System;

namespace SnmpConverter;

internal static class ValueExtensions
{
    internal static SnmpResult<byte[]> ToValue(this byte[] source, ref int offset)
    {
        var lengthResult = source.ToLength(ref offset, x => x < 0, "Incorrect value's length.");

        var value = new byte[lengthResult.Value];
        Buffer.BlockCopy(source, offset, value, 0, lengthResult.Value);
        offset += lengthResult.Value;

        return new SnmpResult<byte[]>(value);
    }

    internal static SnmpResult<byte[]> ToByteArray(this byte[]? value)
    {
        if(value is null)
        {
            value = Array.Empty<byte>();
        }
        return value.ToLength();
    }
}