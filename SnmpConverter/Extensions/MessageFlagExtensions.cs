namespace SnmpConverter;

internal static class MessageFlagExtensions
{
    internal static SnmpMessageFlag ToMessageFlag(this byte[] source, ref int offset)
    {
        source.ToLength(ref offset, SnmpValueType.OctetString).HandleError(x => x != 1, "Incorrect octet string's length.");

        return new SnmpMessageFlag(source[offset++]);
    }

    internal static byte[] ToMessageFlagArray(this SnmpMessageFlag flag)
    {
        return (new[] { flag.Flag }).ToArrayWithLength(SnmpValueType.OctetString);
    }
}