using System;
using Snmp.Model;
using Snmp.Model.Enums;
using Snmp.Model.Exceptions;
using Snmp.Model.Packet;

namespace Snmp.Serializer
{
    public static class SnmpConvert
    {
        public static SnmpBasePacket Serialize(this byte[] source)
        {
            return source.Serialize(0);
        }

        public static SnmpBasePacket Serialize(this byte[] source, int offset)
        {
            if (source[offset++] != (byte)SnmpValueType.CaptionOid) throw new SnmpException("Incorrect format");

            var lengthResult = source.GetLength(ref offset);
            lengthResult.HandleError();
            if (lengthResult.Value < 2) throw new SnmpException("Array too short");

            var versionResult = source.ToVersion(ref offset);
            versionResult.HandleError();

            return versionResult.Value switch
            {
                SnmpVersion.v2c => SerializeV2c(source, offset),
                _ => throw new SnmpException("Unsupported version")
            };
        }

        private static SnmpPacketV2C SerializeV2c(this byte[] source, int offset)
        {

        }


        public static SnmpResult<byte[]> Serialize(this SnmpBasePacket packet)
        {
            var result = packet.IsPacketCorrect();
            result.HandleError();

            return packet switch
            {
                SnmpPacketV2C v2c => v2c.SerializeV2c(),
                _ => throw new SnmpException("Unsupported version")
            };
        }
        
        private static SnmpResult<byte[]> SerializeV2c(this SnmpPacketV2C packet)
        {
            throw new NotImplementedException();
        }



        private static SnmpResult<bool> IsPacketCorrect(this SnmpBasePacket packet)
        {
            if (packet is null)
            {
                return new SnmpResult<bool>("Packet cannot be null");
            }
            
            return new SnmpResult<bool>(true);
        }
    }
}
