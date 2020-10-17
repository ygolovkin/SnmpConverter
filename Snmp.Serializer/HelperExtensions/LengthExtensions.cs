using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Snmp.Model.Exceptions;

namespace Snmp.Serializer.HelperExtensions
{
    public static class LengthExtensions
    {
        public static IEnumerable<byte> LengthEncode(this IEnumerable<byte> source, int length)
        {
            if (length < 0)
            {
                throw new SnmpException("Length cannot be less then 0.");
            }

            var len = BitConverter.GetBytes(length);
            var buffer = new byte[0];

            for (var i = 3; i >= 0; i--)
            {
                if (len[i] != SnmpConstants.LOWEST_BYTE || buffer.Any())
                {
                    buffer = buffer.Append(len[i]).ToArray();
                }
            }
            if (!buffer.Any())
            {
                buffer = buffer.Append(SnmpConstants.LOWEST_BYTE).ToArray();
            }

            if (buffer.Length == 1 && (buffer.First() & SnmpConstants.BIGGEST_BYTE) == 0)
            {
                source = source.Append(buffer);
            }
            else
            {
                var encHeader = (byte)buffer.Length;
                encHeader = (byte)(encHeader | SnmpConstants.BIGGEST_BYTE);
                source = source.Append(encHeader);
                source = source.Append(buffer);
            }

            return source;
        }
    }
}
