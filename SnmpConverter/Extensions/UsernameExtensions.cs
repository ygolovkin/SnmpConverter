namespace SnmpConverter;

internal static class UsernameExtensions
{
    internal static SnmpResult<string> ToUsername(this byte[] source, ref int offset)
    {
        return source.ToString(ref offset);
    }
}