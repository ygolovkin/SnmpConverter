using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snmp.Model;
using Snmp.Model.Enums;
using Snmp.Model.Packet;

namespace Snmp.Serializer
{
    internal static class OidExtensions
    {
        internal static SnmpResult<Oid> GetOid(this byte[] source, ref int offset)
        {
            if (source[offset++] != (byte)SnmpValueType.Object_Identifier) return new SnmpResult<Oid>("Incorrect type of oid");

            var lengthResult = source.GetLength(ref offset);
            if(lengthResult.HasError) return new SnmpResult<Oid>(lengthResult.Error);
            
            var length = lengthResult.Value;
            uint first = source[offset++];

            var oid = new Oid { first };

            if (length == 1)
            {
                oid.Add(1);
                oid.Add(3);
            }
            else
            {
                oid.Add(first / 40);
            }

            oid.Add(first % 40);
            length--;
            while (length > 0)
            {
                uint result = 0;
                if ((source[offset] & SnmpConstants.FLAG_BYTE) == 0)
                {
                    result = source[offset];
                    offset++;
                    --length;
                }
                else
                {
                    byte[] bytes = Array.Empty<byte>();
                    var completed = false;
                    do
                    {
                        bytes = bytes.Append((byte)(source[offset] & ~SnmpConstants.FLAG_BYTE));
                        if ((source[offset] & SnmpConstants.FLAG_BYTE) == 0)
                        {
                            completed = true;
                        }
                        offset++;
                        --length;
                    } while (!completed);

                    foreach (var tmp in bytes)
                    {
                        result <<= 7;
                        result |= tmp;
                    }
                }
                oid.Add(result);
            }

            return new SnmpResult<Oid>(oid);
        }

        internal static SnmpResult<byte[]> ToByteArray(this Oid source)
        {
            uint[] array = source.ToArray();
            byte[] bytes = Array.Empty<byte>();
            if (array == null || array.Length < 2)
            {
                array = new uint[2];
                array[0] = array[1] = 0;
            }
            bytes = bytes.Append((byte)(array[0] * 40 + array[1]));

            for (var i = 2; i < array.Length; i++)
            {
                bytes = bytes.Append(EncodeInstance(array[i]));
            }

            var lengthResult = bytes.Length.GetLength();
            if(lengthResult.HasError) return new SnmpResult<byte[]>(lengthResult.Error);

            var result = new[] {(byte) SnmpValueType.Object_Identifier}
                .Concat(lengthResult.Value)
                .Concat(bytes)
                .ToArray();

            return new SnmpResult<byte[]>(result);
        }

        private static byte[] EncodeInstance(uint number)
        {
            byte[] result = Array.Empty<byte>();
            if (number <= 127)
            {
                result = result.Append((byte)number);
            }
            else
            {
                var value = number;
                byte[] bytes = Array.Empty<byte>();
                while (value != 0)
                {
                    byte[] temp = BitConverter.GetBytes(value);
                    var tFirst = temp[0];
                    if ((tFirst & SnmpConstants.FLAG_BYTE) != 0)
                    {
                        tFirst = (byte)(tFirst & ~SnmpConstants.FLAG_BYTE);
                    }
                    value >>= 7;
                    bytes = bytes.Append(tFirst);
                }
                for (var i = bytes.Length - 1; i >= 0; i--)
                {
                    result = i > 0 
                        ? result.Append((byte)(bytes[i] | SnmpConstants.FLAG_BYTE)) 
                        : result.Append(bytes[i]);
                }
            }
            return result;
        }
    }
}
