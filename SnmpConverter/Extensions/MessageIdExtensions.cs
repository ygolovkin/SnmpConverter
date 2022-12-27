namespace SnmpConverter;

internal static class MessageIdExtensions
{
    internal static SnmpResult<int> ToMessageId(this byte[] source, ref int offset)
    {
        return source.ToInt32(ref offset);
    }
}