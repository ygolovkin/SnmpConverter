namespace SnmpConverter;

internal static class ErrorIndexExtensions
{
    internal static SnmpResult<int> ToErrorIndex(this byte[] source, ref int offset)
    {
        var result = source.ToInt32(ref offset);
        result.HandleError();
        return result;
    }
}