namespace SnmpConverter;

internal static class MessageMaxSizeExtensions
{
    internal static int ToMessageMaxSize(this byte[] source, ref int offset)
    {
        return source.ToInt(ref offset);
    }

    internal static byte[] ToMessageMaxSizeArray(this int messageMaxSize)
    {
        return messageMaxSize.ToIntArray();
    }
}