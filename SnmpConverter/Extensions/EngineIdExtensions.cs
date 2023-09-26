using System;

namespace SnmpConverter;

internal static class EngineIdExtensions
{
    internal static SnmpEngineId ToEngineId(this byte[] source, ref int offset)
    {
        var length = source.ToLength(ref offset, SnmpValueType.OctetString, x => x!= 0 && x is < 10 or > 24, "Incorrect EngineId's length.");

        var engineId = new byte[length];
        Buffer.BlockCopy(source, offset, engineId, 0, length);
        offset += length;

        return new SnmpEngineId(engineId);
    }

    internal static byte[] ToEngineIdArray(this SnmpEngineId? engineId)
    {
        if(engineId is null)
        {
            engineId = new SnmpEngineId();
        }

        return engineId.ToArray().ToArrayWithLength(SnmpValueType.OctetString);
    }
}