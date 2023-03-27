namespace SnmpConverter;

internal static class VersionExtensions
{
    internal static SnmpResult<SnmpVersion> ToVersion(this byte[] source, ref int offset)
    {
        var value = source.ToInt32(ref offset).HandleError();
        var version = value.ToEnum<SnmpVersion>().HandleError();
        return new SnmpResult<SnmpVersion>(version);
    }

    internal static SnmpResult<byte[]> ToByteArray(this SnmpVersion source)
    {
        return ((int)source).ToByteArray();
    }
}