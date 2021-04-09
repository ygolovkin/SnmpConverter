using Snmp.Model;
using System;

namespace Snmp.Serializer
{
    internal static class LengthExtensions
    {
        internal static SnmpResult<int> GetLength(this byte[] source, ref int offset)
        {
            int length;
            if ((source[offset] & SnmpConstants.FLAG_BYTE) == 0)
            {
                length = source[offset++];
            }
            else
            {
                length = source[offset++] & ~SnmpConstants.FLAG_BYTE;
                var value = 0;
                for (var i = 0; i < length; i++)
                {
                    value <<= 8;
                    value |= source[offset++];
                    if (offset > source.Length || (i < (length - 1) && offset == source.Length))
                    {
                        return new SnmpResult<int>("Incorrect value of length");
                    }
                }
                length = value;
            }
            return new SnmpResult<int>(length);
        }

        internal static SnmpResult<byte[]> GetLength(this int source)
        {
            byte[] length = BitConverter.GetBytes(source);
            byte[] buffer = Array.Empty<byte>();

            for (var i = 3; i >= 0; i--)
            {
                if (length[i] != 0 || buffer.Length > 0)
                {
                    buffer = buffer.Append(length[i]);
                }
            }

            if (buffer.Length == 0)
            {
                buffer = buffer.Append(0);
            }
            if (buffer.Length != 1 || (buffer[0] & SnmpConstants.FLAG_BYTE) != 0)
            {
                var encHeader = (byte)buffer.Length;
                encHeader = (byte)(encHeader | SnmpConstants.FLAG_BYTE);
                buffer = buffer.Prepend(encHeader);
            }
            return new SnmpResult<byte[]>(buffer);
        }
    }
}
