namespace SnmpConverter
{
    public class SnmpVariableBinding
    {
        public SnmpOid? Oid { get; set; }

        public SnmpValueType Type { get; set; }

        public byte[]? Value { get; set; }
    }
}
