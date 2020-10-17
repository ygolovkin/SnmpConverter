using System;
using System.Collections.Generic;
using System.Linq;
using Snmp.Model.Exceptions;

namespace Snmp.Serializer.HelperExtensions
{
    public static class LengthExtensions
    {
        public static IEnumerable<byte> LengthEncode(this int source)
        {
            if (source < 0)
            {
                throw new SnmpException("Length cannot be less then 0.");
            }

            var bytes = BitConverter.GetBytes(source);
            var buffer = new byte[] { };

            for (var i = 3; i >= 0; i--)
            {
                if (bytes[i] != 0 || buffer.Length > 0)
                {
                    buffer = buffer.Append(bytes[i]).ToArray();
                }
            }

            if (buffer.Length == 0)
            {
                buffer = buffer.Append(SnmpConstants.LOWEST_BYTE).ToArray();
            }
            if (buffer.Length != 1 || (buffer.First() & SnmpConstants.BIGGEST_BYTE) != 0)
            {
                var encHeader = (byte)buffer.Length;
                encHeader = (byte)(encHeader | SnmpConstants.BIGGEST_BYTE);
                buffer = buffer.Prepend(encHeader).ToArray();
            }
            return buffer;
        }
    }
}
