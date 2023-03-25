namespace SnmpConverter;

internal static class EngineBootsExtensions
{
    internal static SnmpResult<int> ToEngineBoots(this byte[] source, ref int offset)
    {
        return source.ToInt32(ref offset);
    }
}