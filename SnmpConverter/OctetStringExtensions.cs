using System;
using System.Text;

namespace SnmpConverter;

internal static class OctetStringExtensions
{
    internal static SnmpResult<string> ToString(this byte[] source, ref int offset)
    {
        var unwrapResult = source.ToLength(ref offset, SnmpValueType.OctetString);

        var octetString = new byte[unwrapResult.Value];
        Buffer.BlockCopy(source, offset, octetString, 0, unwrapResult.Value);
        offset += unwrapResult.Value;
        return new SnmpResult<string>(value: Encoding.UTF8.GetString(octetString));
    }

    internal static SnmpResult<byte[]> ToByteArray(this string? source)
    {
        return Encoding.UTF8.GetBytes(source ?? string.Empty).ToLength(SnmpValueType.OctetString);
    }
}