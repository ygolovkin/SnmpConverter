namespace SnmpConverter;

internal static class MessageSecurityModelExtensions
{
    internal static int ToMessageSecurityModel(this byte[] source, ref int offset)
    {
        return source.ToInt(ref offset);
    }

    internal static byte[] ToMessageSecurityModelArray(this int messageSecurityModel)
    {
        return messageSecurityModel.ToIntArray();
    }
}