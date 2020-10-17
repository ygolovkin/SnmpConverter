using System;
using System.Collections.Generic;
using System.Linq;
using Snmp.Model.Enums;
using Snmp.Serializer.HelperExtensions;

namespace Snmp.Serializer.ParsingExtensions
{
    internal static class VersionsExtensions
    {
        internal static IEnumerable<byte> VersionEncode(this SnmpVersion source)
        {
            var buffer = ((int) source).IntegerEncode().ToArray();
            return buffer.EncodeType(SnmpValueType.Integer);
        }
    }
}
