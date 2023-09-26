namespace SnmpConverter;

internal static class ErrorIndexExtensions
{
    internal static int ToErrorIndex(this byte[] source, ref int offset)
    {
        return source.ToInt(ref offset);
    }

    internal static byte[] ToErrorIndexArray(this int errorIndex)
    {
        return errorIndex.ToIntArray();
    }
}