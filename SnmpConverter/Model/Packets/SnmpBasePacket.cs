using System.Collections.Generic;

namespace SnmpConverter
{
    public abstract class SnmpBasePacket
    {
        public SnmpVersion Version { get; protected set; }

        public SnmpPduType PduType { get; set; }

        public int RequestId { get; set; }

        public SnmpErrorStatus ErrorStatus { get; set; }

        public int ErrorIndex { get; set; }

        public ICollection<VariableBinding> VariableBindings { get; set; }

        protected SnmpBasePacket()
        {
            VariableBindings = new List<VariableBinding>();
        }
    }
}
