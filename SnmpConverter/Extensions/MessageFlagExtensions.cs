namespace SnmpConverter;

internal static class MessageFlagExtensions
{
    internal static SnmpResult<SnmpMessageFlag> ToMessageFlag(this byte[] source, ref int offset)
    {
        source.ToLength(ref offset, SnmpValueType.OctetString, x => x != 1, "Incorrect octet string's length.");

        return new SnmpResult<SnmpMessageFlag>(new SnmpMessageFlag(source[offset++]));
    }

    internal static SnmpResult<byte[]> ToByteArray(this SnmpMessageFlag flag)
    {
        return (new[] { flag.Flag }).ToLength(SnmpValueType.OctetString);
    }
}