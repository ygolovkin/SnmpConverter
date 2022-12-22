using System;

namespace SnmpConverter;

internal static class IntegerExtensions
{
    internal static SnmpResult<byte[]> ToByteArray(this int source)
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
                if (buffer.Length > 0 || bytes[i] != Constants.LastByte)
                {
                    buffer = buffer.Append(bytes[i]);
                }
            }

            if (buffer.Length == 0)
            {
                buffer = buffer.Append(Constants.LastByte);
            }

            if ((buffer[0] & Constants.HighByte) == 0)
            {
                buffer = buffer.Prepend(Constants.LastByte);
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
            else if ((buffer[0] & Constants.HighByte) != 0)
            {
                buffer = buffer.Prepend(0);
            }
        }

        if (buffer.Length > 1 && buffer[0] == Constants.LastByte && (buffer[1] & Constants.HighByte) != 0)
        {
            buffer = buffer.Prepend(0);
        }

        buffer.Length.ToLength();

        return buffer.ToLength(SnmpValueType.Integer);
    }

    internal static SnmpResult<int> ToInt32(this byte[] source, ref int offset)
    {
        var unwrapResult = source.ToLength(ref offset, SnmpValueType.Integer);
        if (unwrapResult.HasError)
        {
            return unwrapResult;
        }

        if (unwrapResult.Value > 5)
        {
            return new SnmpResult<int>("Incorrect integer length");
        }

        var length = unwrapResult.Value;

        var isNegative = (source[offset] & Constants.HighByte) != 0;

        if (source[offset] == Constants.HighByte 
            && length > 2 
            && source[offset + 1] == Constants.LastByte 
            && (source[offset + 2] & Constants.HighByte) != 0)
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