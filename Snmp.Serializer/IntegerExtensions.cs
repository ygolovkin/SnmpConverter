using System;
using System.Collections.Generic;
using System.Linq;

namespace Snmp.Serializer
{
    internal static class IntegerExtensions
    {
        internal static byte[] ToByteArray(this int source)
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

                if ((buffer.First() & SnmpConstants.SNMP_BIGGEST_BYTE) == 0)
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
                    if ((buffer.First() & SnmpConstants.SNMP_BIGGEST_BYTE) != 0)
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
                (buffer[1] & SnmpConstants.SNMP_BIGGEST_BYTE) != 0)
            {
                buffer = buffer.Prepend(SnmpConstants.LOWEST_BYTE).ToArray();
            }

            return buffer;
        }

    }
}