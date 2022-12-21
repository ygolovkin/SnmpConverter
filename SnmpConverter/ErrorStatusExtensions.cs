namespace SnmpConverter;

internal static class ErrorStatusExtensions
{
    internal const int MinValue = 0;
    internal const int MaxValue = 18;

    internal static SnmpResult<SnmpErrorStatus> ToErrorStatus(this byte[] source, ref int offset)
    {
        var intResult = source.ToInt32(ref offset);

        if (intResult.HasError)
        {
            return new SnmpResult<SnmpErrorStatus>(intResult.Error);
        }

        if (intResult.Value is < MinValue or > MaxValue)
        {
            return new SnmpResult<SnmpErrorStatus>("Incorrect value of error status");
        }

        return new SnmpResult<SnmpErrorStatus>((SnmpErrorStatus) intResult.Value);
    }

    internal static SnmpResult<byte[]> ToByteArray(this SnmpErrorStatus status)
    {
        var intResult = ((int)status).ToByteArray();
        return intResult.HasError ? new SnmpResult<byte[]>(intResult.Error) : intResult;
    }
}