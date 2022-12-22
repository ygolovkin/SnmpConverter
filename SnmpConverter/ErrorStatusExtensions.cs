namespace SnmpConverter;

internal static class ErrorStatusExtensions
{
    internal static SnmpResult<SnmpErrorStatus> ToErrorStatus(this byte[] source, ref int offset)
    {
        var result = source.ToEnum<SnmpErrorStatus>(ref offset);
        result.HandleError();
        return result;
    }

    internal static SnmpResult<byte[]> ToByteArray(this SnmpErrorStatus status)
    {
        var intResult = ((int)status).ToByteArray();
        intResult.HandleError();
        return intResult;
    }
}