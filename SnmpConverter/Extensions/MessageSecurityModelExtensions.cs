namespace SnmpConverter;

internal static class MessageSecurityModelExtensions
{
    internal static SnmpResult<int> ToMessageSecurityModel(this byte[] source, ref int offset)
    {
        return source.ToInt32(ref offset);
    }
}