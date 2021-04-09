using System;
using System.Linq;
using Snmp.Model;
using Snmp.Model.Enums;

namespace Snmp.Serializer
{
    internal static class IntegerExtensions
    {
        internal static SnmpResult<byte[]> ToByteArray(this int source)
        {
            byte[] bytes = BitConverter.GetBytes(source);

            byte[] tmp = Array.Empty<byte>();
            if (source < 0)
            {
                for (var i = 3; i >= 0; i--)
                {
                    if (tmp.Length > 0 || bytes[i] != 0xff)
                    {
                        tmp = tmp.Append(bytes[i]);
                    }
                }

                if (tmp.Length == 0)
                {
                    tmp = tmp.Append(0xff);
                }

                if ((tmp[0] & 0x80) == 0)
                {
                    tmp = tmp.Prepend(0xff);
                }
            }
            else if (source == 0)
            {
                tmp = tmp.Append(0);
            }
            else
            {
                for (var i = 3; i >= 0; i--)
                {
                    if (bytes[i] != 0 || tmp.Length > 0)
                    {
                        tmp = tmp.Append(bytes[i]);
                    }
                }

                if (tmp.Length == 0)
                {
                    tmp = tmp.Append(0);
                }
                else if ((tmp[0] & 0x80) != 0)
                {
                    tmp = tmp.Prepend(0);
                }
            }

            if (tmp.Length > 1 && tmp[0] == 0xff && (tmp[1] & 0x80) != 0)
            {
                tmp = tmp.Prepend(0);
            }

            var lengthResult = tmp.Length.GetLength();
            if(lengthResult.HasError) return new SnmpResult<byte[]>(lengthResult.Error);

            var result = new[] { (byte)SnmpValueType.OctetString }
                .Concat(lengthResult.Value)
                .Concat(tmp)
                .ToArray();

            return new SnmpResult<byte[]>(result);
        }

        internal static SnmpResult<int> GetInt(this byte[] source, ref int offset)
        {
            if (source[offset++] != (byte)SnmpValueType.Integer) return new SnmpResult<int>("Incorrect type of integer");

            var lengthResult = source.GetLength(ref offset);
            if (lengthResult.HasError) return lengthResult;
            
            if (lengthResult.Value > 5) return new SnmpResult<int>("Incorrect integer length");

            var length = lengthResult.Value;

            var isNegative = (source[offset] & SnmpConstants.FLAG_BYTE) != 0;

            if (source[offset] == 0x80 && length > 2 && (source[offset + 1] == 0xff && (source[offset + 2] & 0x80) != 0))
            {
                offset += 1;
                length -= 1;
            }

            var value = isNegative ? -1 : 0;

            for (var i = 0; i < length; i++)
            {
                value <<= 8;
                value |= source[offset++];
            }
            return new SnmpResult<int>(value);
        }
    }
}