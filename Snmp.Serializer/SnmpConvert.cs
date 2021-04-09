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
            lengthResult.HandleError(length => length < 2, "Array too short");

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
            var packet = new SnmpPacketV2C();

            var communityResult = source.GetString(ref offset);
            communityResult.HandleError();
            packet.Community = communityResult.Value;

            var typeRequestResult = source.GetTypeRequest(ref offset);
            typeRequestResult.HandleError();
            packet.TypeRequest = typeRequestResult.Value;

            var lengthResult = source.GetLength(ref offset);
            lengthResult.HandleError();

            var requestIdResult = source.GetInt(ref offset);
            requestIdResult.HandleError();
            packet.RequestId = requestIdResult.Value;

            var errorStatusResult = source.GetErrorStatus(ref offset);
            errorStatusResult.HandleError();
            packet.ErrorStatus = errorStatusResult.Value;

            var errorIndexResult = source.GetInt(ref offset);
            errorIndexResult.HandleError();
            packet.ErrorIndex = errorIndexResult.Value;

            var variableBindingsResult = source.GetVariableBinidings(ref offset);
            variableBindingsResult.HandleError();
            packet.VariableBinidings = variableBindingsResult.Value;

            return packet;
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
