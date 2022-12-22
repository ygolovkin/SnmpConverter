using System;

namespace SnmpConverter;

internal static class ValueExtensions
{
    internal static SnmpResult<byte[]> ToValue(this byte[] source, ref int offset)
    {
        var lengthResult = source.ToLength(ref offset);

        var value = new byte[lengthResult.Value];
        Buffer.BlockCopy(source, offset, value, 0, lengthResult.Value);
        offset += lengthResult.Value;

        return new SnmpResult<byte[]>(value);
    }
}