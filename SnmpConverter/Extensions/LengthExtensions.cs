using System;
using System.Linq;

namespace SnmpConverter;

internal static class LengthExtensions
{
    internal static SnmpResult<int> ToLength(this byte[] source, ref int offset, Func<int, bool>? predicate, string message)
    {
        var result = source.GetLength(ref offset);
        result.HandleError(predicate, message);
        return result;
    }

    internal static SnmpResult<int> ToLength(this byte[] source, ref int offset)
    {
        var result = source.GetLength(ref offset);
        result.HandleError();
        return result;
    }

    internal static SnmpResult<byte[]> ToLength(this int source)
    {
        var result = source.GetLength();
        result.HandleError();
        return result;
    }

    internal static SnmpResult<byte[]> ToLength(this byte[] source, SnmpValueType? valueType = null)
    {
        byte? byteValueType = valueType is null ? null : (byte)valueType;
        return source.ToLength(byteValueType);
    }

    internal static SnmpResult<byte[]> ToLength(this byte[] source, byte? valueType = null)
    {
        var lengthResult = source.Length.GetLength();
        lengthResult.HandleError();
        var valueTypeArray = valueType is null ? Array.Empty<byte>() : new[] { (byte)valueType };

        var result = valueTypeArray
            .Concat(lengthResult.Value)
            .Concat(source)
            .ToArray();

        return new SnmpResult<byte[]>(result);
    }

    internal static SnmpResult<int> ToLength(this byte[] source, ref int offset, SnmpValueType valueType,
    Func<int, bool>? predicate, string message)
    {
        return source.ToLength(ref offset, (byte)valueType, predicate, message);
    }

    internal static SnmpResult<int> ToLength(this byte[] source, ref int offset, byte valueType,
        Func<int, bool>? predicate, string message)
    {
        if (source[offset++] != valueType)
        {
            return new SnmpResult<int>($"Incorrect type of {valueType}");
        }

        var length = source.ToLength(ref offset);
        length.HandleError(predicate, message);
        return length;
    }

    private static SnmpResult<int> GetLength(this byte[] source, ref int offset)
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

    private static SnmpResult<byte[]> GetLength(this int source)
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
}