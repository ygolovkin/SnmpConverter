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
            buffer = buffer.Append((byte)source).ToArray();
            return buffer.LengthEncode(buffer.Length);
        }
    }
}
