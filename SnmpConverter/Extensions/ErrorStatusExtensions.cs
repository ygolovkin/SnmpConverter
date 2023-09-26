namespace SnmpConverter;

internal static class ErrorStatusExtensions
{
    internal static SnmpErrorStatus ToErrorStatus(this byte[] source, ref int offset)
    {
        var errorStatus = source.ToInt(ref offset);
        return errorStatus.ToEnum<SnmpErrorStatus>();
    }

    internal static byte[] ToErrorStatusArray(this SnmpErrorStatus status)
    {
        return ((int)status).ToIntArray();
    }
}