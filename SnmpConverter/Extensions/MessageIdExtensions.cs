namespace SnmpConverter;

internal static class MessageIdExtensions
{
    internal static int ToMessageId(this byte[] source, ref int offset)
    {
        return source.ToInt(ref offset);
    }

    internal static byte[] ToMessageIdArray(this int messageId)
    {
        return messageId.ToIntArray();
    }
}