using System;
using System.Text;

namespace SnmpConverter;

internal static class EngineIdExtensions
{    
    internal static SnmpResult<SnmpEngineId> ToEngineId(this byte[] source, ref int offset)
    {
        var length = source.ToLength(ref offset, SnmpValueType.OctetString, x => x!= 0 && x is < 10 or > 24,
            "Incorrect EngineId's length.");

        var engineId = new byte[length.Value];
        Buffer.BlockCopy(source, offset, engineId, 0, length.Value);
        offset += length.Value;

        return new SnmpResult<SnmpEngineId>(new SnmpEngineId(engineId));
    }
    
}