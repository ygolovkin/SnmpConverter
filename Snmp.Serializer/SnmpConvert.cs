using System;
using System.Collections.Generic;
using Snmp.Model.Packet;

namespace Snmp.Serializer
{
    public static class SnmpConvert
    {
        public static SnmpBasePacket Serialize(this IEnumerable<byte> source)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<byte> Serialize(this SnmpBasePacket packet)
        {
            throw new NotImplementedException();
        }
    }
}
