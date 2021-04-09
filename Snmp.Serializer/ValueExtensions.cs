using System;
using Snmp.Model;

namespace Snmp.Serializer
{
    internal static class ValueExtensions
    {
        internal static SnmpResult<byte[]> GetValue(this byte[] source, ref int offset)
        {
            var lengthResult = source.GetLength(ref offset);
            if (lengthResult.HasError) return new SnmpResult<byte[]>(lengthResult.Error);
            var length = lengthResult.Value;

            var value = new byte[length];
            Buffer.BlockCopy(source, offset, value, 0, length);
            offset += length;

            return new SnmpResult<byte[]>(value);
        }
    }
}