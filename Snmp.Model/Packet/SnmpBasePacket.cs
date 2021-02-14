using Snmp.Model.Enums;

namespace Snmp.Model.Packet
{
    public abstract class SnmpBasePacket
    {
        public SnmpVersion Version { get; protected set; }
    }
}
