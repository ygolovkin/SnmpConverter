using System;
using Snmp.Model;
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

            if (result)
            {
                return packet switch
                {
                    SnmpPacketV2C v2c => v2c.Serialize(),
                    _ => new SnmpResult<byte[]>("Incorrect packet format")
                };
            }

            return new SnmpResult<byte[]>(result.Error);
        }

        private static SnmpResult<byte[]> Serialize(this SnmpPacketV2C packet)
        {
            throw new NotImplementedException();
        }
    }
}
