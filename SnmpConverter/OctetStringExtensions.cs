using System;
using System.Text;

namespace SnmpConverter;

internal static class OctetStringExtensions
{
    internal static SnmpResult<string> ToString(this byte[] source, ref int offset)
    {
        var length = source.ToLength(ref offset, SnmpValueType.OctetString, x => x < 0,
            "Incorrect octet string's length.");

        var octetString = new byte[length.Value];
        Buffer.BlockCopy(source, offset, octetString, 0, length.Value);
        offset += length.Value;
        return new SnmpResult<string>(value: Encoding.UTF8.GetString(octetString));
    }

    internal static SnmpResult<byte[]> ToByteArray(this string? source)
    {
        return Encoding.UTF8.GetBytes(source ?? string.Empty).ToLength(SnmpValueType.OctetString);
    }
}