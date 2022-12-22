namespace SnmpConverter;

internal static class ErrorStatusExtensions
{
    internal static SnmpResult<SnmpErrorStatus> ToErrorStatus(this byte[] source, ref int offset)
    {
        var result = source.ToInt32(ref offset);
        return result.Value.ToEnum<SnmpErrorStatus>();
    }

    internal static SnmpResult<byte[]> ToByteArray(this SnmpErrorStatus status)
    {
        var intResult = ((int)status).ToByteArray();
        intResult.HandleError();
        return intResult;
    }
}