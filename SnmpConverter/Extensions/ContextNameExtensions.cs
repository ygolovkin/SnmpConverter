namespace SnmpConverter;

internal static class ContextNameExtensions
{
    internal static SnmpResult<string> ToContextName(this byte[] source, ref int offset)
    {
        return source.ToString(ref offset);
    }
}