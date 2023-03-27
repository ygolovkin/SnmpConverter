namespace SnmpConverter;

/// <summary>
/// SNMP value type.
/// </summary>
public enum SnmpValueType : byte
{
    /// <summary>
    /// Signed 32-bit integer
    /// </summary>
    Integer = 0x02,

    /// <summary>
    /// Signed 32-bit integer
    /// </summary>
    Integer32 = 0x02,

    /// <summary>
    /// Octets of binary or textual information
    /// </summary>
    OctetString = 0x04,

    /// <summary>
    /// Collection of labeled bits
    /// </summary>
    Bits = 0x04,
    
    /// <summary>
    /// Null
    /// </summary>
    Null = 0x05,

    /// <summary>
    /// Object identifier
    /// </summary>
    ObjectIdentifier = 0x06,

    /// <summary>
    /// IPv4 address as a string of 4 octets
    /// </summary>
    IpAddress = 0x40,

    /// <summary>
    /// Value which represents a count
    /// </summary>
    Counter = 0x41,

    /// <summary>
    /// Value which represents a count
    /// </summary>
    Counter32 = 0x41,

    /// <summary>
    /// Non-negative integer
    /// </summary>
    Gauge = 0x42,

    /// <summary>
    /// Non-negative integer
    /// </summary>
    Gauge32 = 0x42,

    /// <summary>
    /// Non-negative integer
    /// </summary>
    Unsigned32 = 0x42,

    /// <summary>
    /// Elapsed time
    /// </summary>
    TimeTicks = 0x43,

    /// <summary>
    /// Octets of binary information
    /// </summary>
    Opaque = 0x44,

    /// <summary>
    /// Network address of any type
    /// </summary>
    NsapAddress = 0x45,

    /// <summary>
    /// Value which represents a count
    /// </summary>
    Counter64 = 0x46,

    /// <summary>
    /// Unsigned 16-bit integer
    /// </summary>
    UInteger32 = 0x47
}