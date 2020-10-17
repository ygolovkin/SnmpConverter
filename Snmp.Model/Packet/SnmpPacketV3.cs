using Snmp.Model.Enums;

namespace Snmp.Model.Packet
{
    public class SnmpPacketV3 : SnmpBasePacket
    {
        public SnmpPacketV3()
        {
            Version = SnmpVersion.v3;
        }
    }
}