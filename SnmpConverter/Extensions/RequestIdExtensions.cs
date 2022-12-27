namespace SnmpConverter;

internal static class RequestIdExtensions
{
    internal static SnmpResult<int> ToRequestId(this byte[] source, ref int offset)
    {
        return source.ToInt32(ref offset);
    }
}