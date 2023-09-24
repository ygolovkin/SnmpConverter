namespace SnmpConverter;

internal static class ErrorStatusExtensions
{
    internal static SnmpResult<SnmpErrorStatus> ToErrorStatus(this byte[] source, ref int offset)
    {
        var errorStatus = source.ToInt32(ref offset).HandleError();
        return errorStatus.ToEnum<SnmpErrorStatus>();
    }

    internal static SnmpResult<byte[]> ToByteArray(this SnmpErrorStatus status)
    {
        return ((int)status).ToByteArray();
    }
}