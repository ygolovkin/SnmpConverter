using Snmp.Model.Enums;

namespace Snmp.Model.Packet
{
    public class VariableBiniding
    {
        public Oid? Oid { get; set; }

        public SnmpValueType Type { get; set; }

        public byte[]? Value { get; set; }
    }
}
