using Snmp.Model.Enums;

namespace Snmp.Model.Packet
{
    public class SnmpPacketV2C : SnmpBasePacket
    {
        public SnmpPacketV2C()
        {
            Version = SnmpVersion.v2c;
        }
    }
}