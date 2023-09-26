using System;
using System.Text;

namespace SnmpConverter;

internal static class OctetStringExtensions
{
    internal static string ToString(this byte[] source, ref int offset)
    {
        var length = source.ToLength(ref offset, SnmpValueType.OctetString, x => x < 0, "Incorrect octet string's length.");

        var octetString = new byte[length];
        Buffer.BlockCopy(source, offset, octetString, 0, length);
        offset += length;

        return Encoding.UTF8.GetString(octetString);
    }

    internal static byte[] ToStringArray(this string? source)
    {
        return Encoding.UTF8.GetBytes(source ?? string.Empty).ToArrayWithLength(SnmpValueType.OctetString);
    }
}