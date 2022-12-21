namespace SnmpConverter;

internal static class VersionExtensions
{
    internal static SnmpResult<SnmpVersion> ToVersion(this byte[] source, ref int offset)
    {
        var intResult = source.ToInt32(ref offset);
        if (intResult.HasError)
        {
            return new SnmpResult<SnmpVersion>(intResult.Error);
        }
        if (intResult.Value is < 0 or > 2)
        {
            return new SnmpResult<SnmpVersion>("Incorrect value of version");
        }

        return (byte) intResult.Value switch
        {
            (byte)SnmpVersion.V2C => new SnmpResult<SnmpVersion>(SnmpVersion.V2C),
            _ => new SnmpResult<SnmpVersion>("Unsupported version")
        };
    }

    internal static SnmpResult<byte[]> ToByteArray(this SnmpVersion source)
    {
        var intResult = ((int) source).ToByteArray();
        return intResult.HasError ? new SnmpResult<byte[]>(intResult.Error) : intResult;
    }
}