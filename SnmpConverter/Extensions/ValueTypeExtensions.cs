using System;
using System.Linq;

namespace SnmpConverter;

internal static class ValueTypeExtensions
{
    internal static SnmpValueType ToValueType(this byte[] source, ref int offset)
    {
        var valueType = source[offset++];
        if (Enum.GetValues(typeof(SnmpValueType)).Cast<byte>().Contains(valueType))
        {
            throw new SnmpException("Incorrect value of value type.");
        }

        return (SnmpValueType)valueType;
    }

    internal static byte[] ToValueTypeArray(this SnmpValueType valueType)
    {
        return new[] { (byte)valueType };
    }
}