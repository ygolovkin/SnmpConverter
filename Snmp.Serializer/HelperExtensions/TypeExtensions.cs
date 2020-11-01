using System.Linq;
using System.Collections.Generic;
using Snmp.Model.Enums;

namespace Snmp.Serializer.HelperExtensions
{
    internal static class TypeExtensions
    {
        internal static IEnumerable<byte> TypeEncode(this IEnumerable<byte> source, SnmpValueType type)
        {
            return source.Prepend(source.Count().LengthEncode()).Prepend((byte) type);
        }

        internal static byte TypeDecode(this IEnumerable<byte> source, ref int offset, out int length)
        {
            length = source.LengthDecode(ref offset);
            var array = source.ToArray();
            return array[offset++];
        }
    }
}