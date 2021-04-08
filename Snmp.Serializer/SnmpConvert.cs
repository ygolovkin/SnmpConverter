using System;
using Snmp.Model;
using Snmp.Model.Exceptions;
using Snmp.Model.Packet;

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
                    _ => throw new SnmpException("Incorrect packet format")
                };
            }

            throw new SnmpException(result.Error);
        }



        private static SnmpResult<byte[]> Serialize(this SnmpPacketV2C packet)
        {
            throw new NotImplementedException();
        }

        private static SnmpResult<bool> IsCorrect(this SnmpBasePacket packet)
        {
            if (packet is null)
            {
                return new SnmpResult<bool>("Packet cannot be null");
            }
            
            return new SnmpResult<bool>(true);
        }
    }
}
