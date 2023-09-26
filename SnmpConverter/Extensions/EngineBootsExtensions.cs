namespace SnmpConverter;

internal static class EngineBootsExtensions
{
    internal static int ToEngineBoots(this byte[] source, ref int offset)
    {
        return source.ToInt(ref offset);
    }

    internal static byte[] ToEngineBootsArray(this int engineBoots)
    {
        return engineBoots.ToIntArray();
    }
}