namespace SnmpConverter;

internal static class VersionExtensions
{
    internal static SnmpVersion ToVersion(this byte[] source, ref int offset)
    {
        return source.ToInt(ref offset).ToEnum<SnmpVersion>();
    }

    internal static byte[] ToVersionArray(this SnmpVersion source)
    {
        return ((int) source).ToIntArray();
    }
}