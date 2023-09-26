namespace SnmpConverter;

internal static class RequestIdExtensions
{
    internal static int ToRequestId(this byte[] source, ref int offset)
    {
        return source.ToInt(ref offset);
    }

    internal static byte[] ToRequestIdArray(this int requestId)
    {
        return requestId.ToIntArray();
    }
}