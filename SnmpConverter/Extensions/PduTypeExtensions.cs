namespace SnmpConverter;

internal static class PduTypeExtensions
{
    internal static SnmpPduType ToPduType(this byte[] source, ref int offset)
    {
        return source.ToEnum<SnmpPduType>(ref offset);
    }

    internal static byte[] ToPduTypeArray(this SnmpPduType source)
    {
        return new[] {(byte) source};
    }
}