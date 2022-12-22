namespace SnmpConverter;

internal static class RequestIdExtensions
{
    internal static SnmpResult<int> ToRequestId(this byte[] source, ref int offset)
    {
        var result = source.ToInt32(ref offset);
        result.HandleError();
        return result;
    }
}