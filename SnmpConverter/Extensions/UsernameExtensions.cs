namespace SnmpConverter;

internal static class UsernameExtensions
{
    internal static string ToUsername(this byte[] source, ref int offset)
    {
        return source.ToString(ref offset);
    }
    internal static byte[] ToUsernameArray(this string? username)
    {
        return username.ToStringArray();
    }
}