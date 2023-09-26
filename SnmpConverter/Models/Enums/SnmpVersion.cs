namespace SnmpConverter;

/// <summary>
/// SNMP Version
/// </summary>
public enum SnmpVersion : byte
{
    /// <summary>
    /// SNMP 1 version
    /// </summary>
    V1 = 0x00,

    /// <summary>
    /// SNMP 2c version
    /// </summary>
    V2C = 0x01,

    /// <summary>
    /// SNMP 2u version
    /// </summary>
    V2U = 0x02,

    /// <summary>
    /// SNMP 3 version
    /// </summary>
    V3 = 0x03
}