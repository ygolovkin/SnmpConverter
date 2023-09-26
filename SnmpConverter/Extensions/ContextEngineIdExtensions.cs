using System;

namespace SnmpConverter;

internal static class ContextEngineIdExtensions
{
    internal static SnmpEngineId ToContextEngineId(this byte[] source, SnmpUser user, ref int offset)
    {
        var length = source.ToLength(ref offset, SnmpValueType.OctetString, x => x!= 0 && x is < 10 or > 24, "Incorrect Context EngineId's length.");

        var engineId = new byte[length];
        Buffer.BlockCopy(source, offset, engineId, 0, length);
        offset += length;

        var contextEngineId = new SnmpEngineId(engineId);
        if(!contextEngineId.IsEmpty && !contextEngineId.Equals(user.ContextEngineId))
        {
            throw new SnmpException("Incorrect user's Context EngineId.");
        }

        return contextEngineId;
    }
}