namespace SnmpConverter;

internal static class VersionExtensions
{
    internal static SnmpResult<SnmpVersion> ToVersion(this byte[] source, ref int offset)
    {
        var intResult = source.ToInt32(ref offset);
        var versionResult = intResult.Value.ToEnum<SnmpVersion>();
        
        return versionResult.Value switch
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