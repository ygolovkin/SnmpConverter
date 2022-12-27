using System;
using System.Linq;

namespace SnmpConverter;

internal static class ValueTypeExtensions
{
    internal static SnmpResult<SnmpValueType> ToValueType(this byte[] source, ref int offset)
    {
        var valueType = source[offset++];
        return !Enum.GetValues(typeof(SnmpValueType)).Cast<byte>().Contains(valueType) 
            ? new SnmpResult<SnmpValueType>("Incorrect value of value type") 
            : new SnmpResult<SnmpValueType>((SnmpValueType)valueType);
    }

    internal static SnmpResult<byte[]> ToByteArray(this SnmpValueType valueType)
    {
        return new SnmpResult<byte[]>(new[] { (byte)valueType });
    }
}