using Snmp.Model.Enums;

namespace Snmp.Model.Packet
{
    public class SnmpPacketV1 : SnmpBasePacket
    {
        public SnmpPacketV1()
        {
            Version = SnmpVersion.v1;
        }
    }
}