using System;
using System.Linq;

namespace SnmpConverter;

internal static class TypeRequestExtensions
{
    internal static SnmpResult<SnmpTypeRequest> GetTypeRequest(this byte[] source, ref int offset)
    {
        var result = source.ToEnum<SnmpTypeRequest>(ref offset);
        result.HandleError();
        return result;
    }

    internal static SnmpResult<byte[]> ToByteArray(this SnmpTypeRequest source)
    {
        return new SnmpResult<byte[]>(new[] {(byte) source});
    }
}