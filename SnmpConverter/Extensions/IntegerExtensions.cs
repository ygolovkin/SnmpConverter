using System;

namespace SnmpConverter;

internal static class IntegerExtensions
{
    internal static byte[] ToIntArray(this int source)
    {
        var bytes = BitConverter.GetBytes(source);

        var buffer = Array.Empty<byte>();
        if (source == 0)
        {
            buffer = buffer.Append(0);
        }
        else if (source < 0)
        {
            for (var i = 3; i >= 0; i--)
            {
                if (buffer.Length > 0 || bytes[i] != SnmpConstants.LastByte)
                {
                    buffer = buffer.Append(bytes[i]);
                }
            }

            if (buffer.Length == 0)
            {
                buffer = buffer.Append(SnmpConstants.LastByte);
            }

            if ((buffer[0] & SnmpConstants.HighByte) == 0)
            {
                buffer = buffer.Prepend(SnmpConstants.LastByte);
            }
        }
        else
        {
            for (var i = 3; i >= 0; i--)
            {
                if (bytes[i] != 0 || buffer.Length > 0)
                {
                    buffer = buffer.Append(bytes[i]);
                }
            }

            if (buffer.Length == 0)
            {
                buffer = buffer.Append(0);
            }
            else if ((buffer[0] & SnmpConstants.HighByte) != 0)
            {
                buffer = buffer.Prepend(0);
            }
        }

        if (buffer.Length > 1 && buffer[0] == SnmpConstants.LastByte && (buffer[1] & SnmpConstants.HighByte) != 0)
        {
            buffer = buffer.Prepend(0);
        }

        buffer.Length.ToLengthArray();

        return buffer.ToArrayWithLength(SnmpValueType.Integer);
    }

    internal static int ToInt(this byte[] source, ref int offset)
    {
        var length = source.ToLength(ref offset, SnmpValueType.Integer, x => x is < 0 or > 5, "Incorrect Int32 length");

        var isNegative = (source[offset] & SnmpConstants.HighByte) != 0;

        if (source[offset] == SnmpConstants.HighByte 
            && length > 2 
            && source[offset + 1] == SnmpConstants.LastByte 
            && (source[offset + 2] & SnmpConstants.HighByte) != 0)
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
        return value;
    }
}