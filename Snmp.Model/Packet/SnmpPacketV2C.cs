using Snmp.Model.Enums;

namespace Snmp.Model.Packet
{
    public class SnmpPacketV2C : SnmpBasePacket
    {
        public string? Community { get; set; }

        public SnmpPacketV2C() : base()
        {
            Version = SnmpVersion.v2c;
        }
    }
}