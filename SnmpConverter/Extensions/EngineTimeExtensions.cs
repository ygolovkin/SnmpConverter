namespace SnmpConverter;

internal static class EngineTimeExtensions
{
    internal static int ToEngineTime(this byte[] source, ref int offset)
    {
        return source.ToInt(ref offset);
    }

    internal static byte[] ToEngineTimeArray(this int engineTime)
    {
        return engineTime.ToIntArray();
    }
}