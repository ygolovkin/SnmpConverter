using System;
using System.Collections.Generic;
using System.Linq;
using Snmp.Model.Enums;
using Snmp.Model.Exceptions;
using Snmp.Serializer.HelperExtensions;

namespace Snmp.Serializer.ParsingExtensions
{
    internal static class VersionsExtensions
    {
        private const int VERSION_BYTE_LENGTH = 1;

        internal static IEnumerable<byte> VersionEncode(this SnmpVersion source)
        {
            var buffer = ((int) source).IntegerEncode().ToArray();
            return buffer.TypeEncode(SnmpValueType.Integer);
        }

        internal static SnmpVersion VersionDecode(this IEnumerable<byte> source, ref int offset)
        {
            var version = source.TypeDecode(ref offset, out var length);
            if (length != VERSION_BYTE_LENGTH)
            {
                throw new SnmpException("Invalid value of version");
            }

            var isContainsCorrectVersion = Enum.GetValues(typeof(SnmpVersion))
                .Cast<byte>()
                .Any(v => v == version);

            if (!isContainsCorrectVersion)
            {
                throw new SnmpException("Invalid value of version");
            }

            return (SnmpVersion) version;
        }
    }
}