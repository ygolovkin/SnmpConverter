namespace SnmpConverter;

internal static class ErrorIndexExtensions
{
    internal static SnmpResult<int> ToErrorIndex(this byte[] source, ref int offset)
    {
        return source.ToInt32(ref offset);
    }
}