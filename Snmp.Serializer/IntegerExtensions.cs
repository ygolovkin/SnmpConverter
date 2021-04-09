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
            var bytes = BitConverter.GetBytes(source);

            var buffer = Array.Empty<byte>();

            if (source < 0)
            {
                for (var i = 3; i >= 0; i--)
                {
                    if (buffer.Any() || bytes[i] != SnmpConstants.HIGHEST_BYTE)
                    {
                        buffer = buffer.Append(bytes[i]).ToArray();
                    }
                }

                if (!buffer.Any())
                {
                    buffer = buffer.Append(SnmpConstants.HIGHEST_BYTE).ToArray();
                }

                if ((buffer.First() & SnmpConstants.FLAG_BYTE) == 0)
                {
                    buffer = buffer.Prepend(SnmpConstants.HIGHEST_BYTE).ToArray();
                }
            }
            else if (source == 0)
            {
                buffer = buffer.Append(SnmpConstants.LOWEST_BYTE).ToArray();
            }
            else
            {
                for (var i = 3; i >= 0; i--)
                {
                    if (bytes[i] != SnmpConstants.LOWEST_BYTE || buffer.Any())
                    {
                        buffer = buffer.Append(bytes[i]).ToArray();
                    }
                }

                if (buffer.Any())
                {
                    if ((buffer.First() & SnmpConstants.FLAG_BYTE) != 0)
                    {
                        buffer = buffer.Prepend(SnmpConstants.LOWEST_BYTE).ToArray();
                    }
                }
                else
                {
                    buffer = buffer.Append(SnmpConstants.LOWEST_BYTE).ToArray();
                }
            }

            if (buffer.Length > 1 && 
                buffer.First() == SnmpConstants.HIGHEST_BYTE &&
                (buffer[1] & SnmpConstants.FLAG_BYTE) != 0)
            {
                buffer = buffer.Prepend(SnmpConstants.LOWEST_BYTE).ToArray();
            }

            return new SnmpResult<byte[]>(buffer);
        }

        internal static SnmpResult<int> ToInt(this byte[] source, ref int offset)
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