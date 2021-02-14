using System;
using System.Collections.Generic;
using Snmp.Model;
using Snmp.Model.Exceptions;
using Snmp.Model.Packet;
using Snmp.Serializer.CheckExtensions;

namespace Snmp.Serializer
{
    public static class SnmpConvert
    {
        public static SnmpBasePacket Serialize(this byte[] source)
        {
            throw new NotImplementedException();
        }

        public static SnmpResult<byte[]> Serialize(this SnmpBasePacket packet)
        {
            var result = packet.IsCorrect();
            if (!result) return new SnmpResult<byte[]>(result.Error);

            return packet switch
            {
                SnmpPacketV2C v2c => v2c.Serialize(),
                SnmpPacketV3 v3 => v3.Serialize(),
                _ => new SnmpResult<byte[]>("Incorrect packet format")
            };
        }

        private static SnmpResult<byte[]> Serialize(this SnmpPacketV2C packet)
        {
            throw new NotImplementedException();
        }

        private static SnmpResult<byte[]> Serialize(this SnmpPacketV3 packet)
        {
            throw new NotImplementedException();
        }
    }
}
