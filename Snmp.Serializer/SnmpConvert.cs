using System;
using System.Collections.Generic;
using Snmp.Model.Packet;
using Snmp.Serializer.CheckExtensions;

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
            packet.GeneralCheck();

            throw new NotImplementedException();
        }
    }
}
