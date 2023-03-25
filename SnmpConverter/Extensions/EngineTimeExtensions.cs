namespace SnmpConverter;

internal static class EngineTimeExtensions
{
    internal static SnmpResult<int> ToEngineTime(this byte[] source, ref int offset)
    {
        return source.ToInt32(ref offset);
    }
}