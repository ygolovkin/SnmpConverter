using System;
using System.Collections.Generic;
using Snmp.Model.Exceptions;
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

            return packet switch
            {
                SnmpPacketV1 v1 => v1.Serialize(),
                SnmpPacketV2C v2c => v2c.Serialize(),
                SnmpPacketV3 v3 => v3.Serialize(),
                _ => throw new SnmpException("Incorrect packet format")
            };
        }

        private static IEnumerable<byte> Serialize(this SnmpPacketV1 packet)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<byte> Serialize(this SnmpPacketV2C packet)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<byte> Serialize(this SnmpPacketV3 packet)
        {
            throw new NotImplementedException();
        }
    }
}
