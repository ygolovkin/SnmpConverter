namespace Snmp.Model.Enums
{
    public enum SnmpValueType : byte
    {
        Integer = 0x02,
        Integer32 = 0x02,
        OctetString = 0x04,
        Bits = 0x04,
        Null = 0x05,
        Object_Identifier = 0x06,
        IpAddress = 0x40,
        Counter = 0x41,
        Counter32 = 0x41,
        Gauge = 0x42,
        Gauge32 = 0x42,
        Unsigned32 = 0x42,
        TimeTicks = 0x43,
        Opaque = 0x44,
        NsapAddress = 0x45,
        Counter64 = 0x46,
        UInteger32 = 0x47,
    }
}