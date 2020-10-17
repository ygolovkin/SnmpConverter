using System.Collections.Generic;
using System.Linq;
using Snmp.Model.Enums;

namespace Snmp.Serializer.HelperExtensions
{
    internal static class TypeExtensions
    {
        internal static IEnumerable<byte> EncodeType(this IEnumerable<byte> source, SnmpValueType type)
        {
            return source.Prepend(source.Count().LengthEncode()).Prepend((byte) type);
        }
    }
}