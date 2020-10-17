using System;
using System.Collections.Generic;
using Snmp.Model.Enums;

namespace Snmp.Serializer.ParsingExtensions
{
    public static class VersionsExtensions
    {
        public static IEnumerable<byte> Serialize(this SnmpVersion source)
        {
            var buffer = ((int) source).Serialize();

            throw new NotImplementedException();
        }
    }
}
