using Snmp.Serializer.HelperExtensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Snmp.Serializer.ParsingExtensions
{
    public static class IntegerExtensions
    {
        public static IEnumerable<byte> Serialize(this int source)
        {
            var bytes = BitConverter.GetBytes(source);

            var buffer = new byte[0];

            if (source < 0)
            {
                for (var i = 3; i >= 0; i--)
                {
                    if (buffer.Any() || bytes[i] != SnmpConstants.HIGHEST_BYTE)
                    {
                        buffer = buffer.Append(bytes[i]);
                    }
                }

                if (!buffer.Any())
                {
                    buffer = buffer.Append(SnmpConstants.HIGHEST_BYTE);
                }

                if ((buffer.First() & SnmpConstants.BIGGEST_BYTE) == 0)
                {
                    buffer = buffer.Prepend(SnmpConstants.HIGHEST_BYTE);
                }
            }
            else if (source == 0)
            {
                buffer = buffer.Append(SnmpConstants.LOWEST_BYTE);
            }
            else
            {
                for (var i = 3; i >= 0; i--)
                {
                    if (bytes[i] != SnmpConstants.LOWEST_BYTE || buffer.Any())
                    {
                        buffer = buffer.Append(bytes[i]);
                    }
                }

                if (buffer.Any())
                {
                    if ((buffer.First() & SnmpConstants.BIGGEST_BYTE) != 0)
                    {
                        buffer = buffer.Prepend(SnmpConstants.LOWEST_BYTE);
                    }
                }
                else
                {
                    buffer = buffer.Append(SnmpConstants.LOWEST_BYTE);
                }
            }

            if (buffer.Length > 1 && 
                buffer.First() == SnmpConstants.HIGHEST_BYTE &&
                (buffer[1] & SnmpConstants.BIGGEST_BYTE) != 0)
            {
                buffer = buffer.Prepend(SnmpConstants.LOWEST_BYTE);
            }

            return buffer;
        }
    }
}