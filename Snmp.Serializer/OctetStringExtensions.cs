using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snmp.Model;
using Snmp.Model.Enums;

namespace Snmp.Serializer
{
    internal static class OctetStringExtensions
    {
        internal static SnmpResult<string> GetString(this byte[] source, ref int offset)
        {
            if (source[offset++] != (byte)SnmpValueType.OctetString) return new SnmpResult<string>(error:"Incorrect type of octet string");

            var lengthResult = source.GetLength(ref offset);
            if (lengthResult.HasError) return new SnmpResult<string>(error: lengthResult.Error);


            var community = new byte[lengthResult.Value];
            Buffer.BlockCopy(source, offset, community, 0, lengthResult.Value);
            offset += lengthResult.Value;
            return new SnmpResult<string>(value: Encoding.UTF8.GetString(community));
        }

        internal static SnmpResult<byte[]> ToByteArray(this string source)
        {
            var bytes = Encoding.UTF8.GetBytes(source ?? string.Empty);
            var lengthResult = bytes.Length.GetLength();
            if(lengthResult.HasError) return new SnmpResult<byte[]>(lengthResult.Error);

            var result = new [] { (byte)SnmpValueType.OctetString }
                .Concat(lengthResult.Value)
                .Concat(bytes)
                .ToArray();

            return new SnmpResult<byte[]>(result);
        }
    }
}
