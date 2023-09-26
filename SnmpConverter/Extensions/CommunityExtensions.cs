namespace SnmpConverter;

internal static class CommunityExtensions
{
    internal static string ToCommunity(this byte[] source, ref int offset)
    {
        return source.ToString(ref offset);
    }

    internal static byte[] ToCommunityArray(this string? community)
    {
        return community.ToStringArray();
    }
}
