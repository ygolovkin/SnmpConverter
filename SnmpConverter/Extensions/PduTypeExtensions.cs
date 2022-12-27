namespace SnmpConverter;

internal static class PduTypeExtensions
{
    internal static SnmpResult<SnmpPduType> ToPduType(this byte[] source, ref int offset)
    {
        var result = source.ToEnum<SnmpPduType>(ref offset);
        result.HandleError();
        return result;
    }

    internal static SnmpResult<byte[]> ToByteArray(this SnmpPduType source)
    {
        return new SnmpResult<byte[]>(new[] {(byte) source});
    }
}