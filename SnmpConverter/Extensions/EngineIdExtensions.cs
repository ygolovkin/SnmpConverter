using System;

namespace SnmpConverter;

internal static class EngineIdExtensions
{
    internal static SnmpResult<SnmpEngineId> ToEngineId(this byte[] source, ref int offset)
    {
        var length = source.ToLength(ref offset, SnmpValueType.OctetString)
            .HandleError(x => x!= 0 && x is < 10 or > 24, "Incorrect EngineId's length.");

        var engineId = new byte[length];
        Buffer.BlockCopy(source, offset, engineId, 0, length);
        offset += length;

        return new SnmpResult<SnmpEngineId>(new SnmpEngineId(engineId));
    }

    internal static SnmpResult<byte[]> ToByteArray(this SnmpEngineId? engineId)
    {
        engineId ??= new SnmpEngineId();

        return engineId.ToArray().ToLength(SnmpValueType.OctetString);
    }
}