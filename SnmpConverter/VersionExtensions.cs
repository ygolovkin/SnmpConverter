namespace SnmpConverter;

internal static class VersionExtensions
{
    internal static SnmpResult<SnmpVersion> ToVersion(this byte[] source, ref int offset)
    {
        var result = source.ToEnum<SnmpVersion>(ref offset);

        return result.Value switch
        {
            SnmpVersion.V2C => new SnmpResult<SnmpVersion>(SnmpVersion.V2C),
            _ => new SnmpResult<SnmpVersion>("Unsupported version")
        };
    }

    internal static SnmpResult<byte[]> ToByteArray(this SnmpVersion source)
    {
        var intResult = ((int) source).ToByteArray();
        return intResult.HasError ? new SnmpResult<byte[]>(intResult.Error) : intResult;
    }
}