using System;
using System.Linq;

namespace SnmpConverter;

internal static class LengthExtensions
{
    internal static SnmpResult<byte[]> ToLength(this byte[] source, SnmpValueType valueType)
    {
        return source.ToLength((byte)valueType);
    }

    internal static SnmpResult<byte[]> ToLength(this byte[] source, byte? valueType = null)
    {
        var length = source.Length.ToLength().HandleError();
        var valueTypeArray = valueType is null ? Array.Empty<byte>() : new[] { (byte)valueType };

        var result = valueTypeArray
            .Concat(length)
            .Concat(source)
            .ToArray();

        return new SnmpResult<byte[]>(result);
    }

    internal static SnmpResult<byte[]> ToLength(this int source)
    {
        var length = BitConverter.GetBytes(source);
        var buffer = Array.Empty<byte>();

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
        if (buffer.Length != 1 || (buffer[0] & SnmpConstants.HighByte) != 0)
        {
            var header = (byte)buffer.Length;
            header = (byte)(header | SnmpConstants.HighByte);
            buffer = buffer.Prepend(header);
        }
        return new SnmpResult<byte[]>(buffer);
    }


    internal static SnmpResult<int> ToLength(this byte[] source, ref int offset, SnmpValueType valueType)
    {
        return source.ToLength(ref offset, (byte)valueType);
    }

    internal static SnmpResult<int> ToLength(this byte[] source, ref int offset, byte valueType)
    {
        if (source[offset++] != valueType)
        {
            return new SnmpResult<int>($"Incorrect type of {valueType}");
        }

        return source.ToLength(ref offset);
    }

    internal static SnmpResult<int> ToLength(this byte[] source, ref int offset)
    {
        int length;
        if ((source[offset] & SnmpConstants.HighByte) == 0)
        {
            length = source[offset++];
        }
        else
        {
            length = source[offset++] & ~SnmpConstants.HighByte;
            var value = 0;
            for (var i = 0; i < length; i++)
            {
                value <<= 8;
                value |= source[offset++];
                if (offset > source.Length || (i < length - 1 && offset == source.Length))
                {
                    return new SnmpResult<int>("Incorrect value of length");
                }
            }
            length = value;
        }
        return new SnmpResult<int>(length);
    }
}