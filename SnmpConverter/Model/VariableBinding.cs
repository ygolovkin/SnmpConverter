namespace SnmpConverter
{
    public class VariableBinding
    {
        public Oid? Oid { get; set; }

        public SnmpValueType Type { get; set; }

        public byte[]? Value { get; set; }
    }
}
