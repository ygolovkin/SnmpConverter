using System;
using System.Linq;

namespace SnmpConverter;

internal static class LengthExtensions
{
    internal static int ToLength(this byte[] source, ref int offset, Func<int, bool>? predicate, string message)
    {
        var length = source.ToLength(ref offset);
        if (predicate is not null && predicate(length))
        {
            throw new SnmpException(message);
        }
        return length;
    }

    internal static byte[] ToArrayWithLength(this byte[] source)
    {
        byte? byteValueType = null;
        return source.ToArrayWithLength(byteValueType);
    }

    internal static byte[] ToArrayWithLength(this byte[] source, SnmpValueType valueType)
    {
        return source.ToArrayWithLength((byte)valueType);
    }

    internal static byte[] ToArrayWithLength(this byte[] source, byte? valueType)
    {
        var length = source.Length.ToLengthArray();
        var valueTypeArray = valueType is null ? Array.Empty<byte>() : new[] { (byte)valueType };

        return valueTypeArray
            .Concat(length)
            .Concat(source)
            .ToArray();
    }

    internal static int ToLength(this byte[] source, ref int offset, SnmpValueType valueType, Func<int, bool>? predicate, string message)
    {
        return source.ToLength(ref offset, (byte)valueType, predicate, message);
    }

    internal static int ToLength(this byte[] source, ref int offset, byte valueType, Func<int, bool>? predicate, string message)
    {
        if (source[offset++] != valueType)
        {
            throw new SnmpException($"Incorrect type of {valueType}");
        }

        return source.ToLength(ref offset, predicate, message);
    }

    internal static int ToLength(this byte[] source, ref int offset)
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
                    throw new SnmpException("Incorrect value of length.");
                }
            }
            length = value;
        }
        return length;
    }

    internal static byte[] ToLengthArray(this int source)
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
        return buffer;
    }
}