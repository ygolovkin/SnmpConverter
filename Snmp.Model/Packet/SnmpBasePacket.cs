using System.Collections.Generic;
using Snmp.Model.Enums;

namespace Snmp.Model.Packet
{
    public abstract class SnmpBasePacket
    {
        public SnmpVersion Version { get; protected set; }

        public int RequestId { get; set; }

        public SnmpTypeRequest TypeRequest { get; set; }

        public SnmpErrorStatus ErrorStatus { get; set; }

        public int ErrorIndex { get; set; }

        public ICollection<VariableBiniding> VariableBinidings { get; set; }

        protected SnmpBasePacket()
        {
            VariableBinidings = new List<VariableBiniding>();
        }
    }
}
