namespace SnmpConverter;

internal static class MessageMaxSizeExtensions
{
    internal static SnmpResult<int> ToMessageMaxSize(this byte[] source, ref int offset)
    {
        return source.ToInt32(ref offset);
    }
}