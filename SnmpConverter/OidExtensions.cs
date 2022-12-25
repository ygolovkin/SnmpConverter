using System;

namespace SnmpConverter;

internal static class OidExtensions
{
    internal static SnmpResult<Oid> ToOid(this byte[] source, ref int offset)
    {
        var length = source.ToLength(ref offset, SnmpValueType.ObjectIdentifier, x => x < 0,
            "Incorrect Oid's length.").Value;

        uint first = source[offset++];

        var oid = new Oid { first };

        if (length == 1)
        {
            oid.Add(1);
            oid.Add(3);
        }
        else
        {
            oid.Add(first / 40);
        }

        oid.Add(first % 40);
        length--;
        while (length > 0)
        {
            uint result = 0;
            if ((source[offset] & Constants.HighByte) == 0)
            {
                result = source[offset];
                offset++;
                --length;
            }
            else
            {
                var bytes = Array.Empty<byte>();
                var completed = false;
                do
                {
                    bytes = bytes.Append((byte)(source[offset] & ~Constants.HighByte));
                    if ((source[offset] & Constants.HighByte) == 0)
                    {
                        completed = true;
                    }
                    offset++;
                    --length;
                } while (!completed);

                foreach (var tmp in bytes)
                {
                    result <<= 7;
                    result |= tmp;
                }
            }
            oid.Add(result);
        }

        return new SnmpResult<Oid>(oid);
    }

    internal static SnmpResult<byte[]> ToByteArray(this Oid oid)
    {
        var array = oid.ToArray();
        var bytes = Array.Empty<byte>();
        if (array.Length < 2)
        {
            array = new uint[2];
            array[0] = array[1] = 0;
        }
        bytes = bytes.Append((byte)(array[0] * 40 + array[1]));

        for (var i = 2; i < array.Length; i++)
        {
            bytes = bytes.Append(EncodeInstance(array[i]));
        }

        return bytes.ToLength(SnmpValueType.ObjectIdentifier);
    }

    private static byte[] EncodeInstance(uint number)
    {
        var result = Array.Empty<byte>();
        if (number <= 127)
        {
            result = result.Append((byte)number);
        }
        else
        {
            var value = number;
            var bytes = Array.Empty<byte>();
            while (value != 0)
            {
                var temp = BitConverter.GetBytes(value);
                var tFirst = temp[0];
                if ((tFirst & Constants.HighByte) != 0)
                {
                    tFirst = (byte)(tFirst & ~Constants.HighByte);
                }
                value >>= 7;
                bytes = bytes.Append(tFirst);
            }
            for (var i = bytes.Length - 1; i >= 0; i--)
            {
                result = i > 0 
                    ? result.Append((byte)(bytes[i] | Constants.HighByte)) 
                    : result.Append(bytes[i]);
            }
        }
        return result;
    }
}