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

        internal static byte[] GetLength(int source)
        {
            byte[] len = BitConverter.GetBytes(source);
            byte[] buf = Array.Empty<byte>();

            for (var i = 3; i >= 0; i--)
            {
                if (len[i] != 0 || buf.Length > 0)
                {
                    buf = buf.Append(len[i]);
                }
            }

            if (buf.Length == 0)
            {
                buf = buf.Append(0);
            }
            if (buf.Length != 1 || (buf[0] & SnmpConstants.FLAG_BYTE) != 0)
            {
                var encHeader = (byte)buf.Length;
                encHeader = (byte)(encHeader | SnmpConstants.FLAG_BYTE);
                buf = buf.Prepend(encHeader);
            }
            return buf;
        }
    }
}
