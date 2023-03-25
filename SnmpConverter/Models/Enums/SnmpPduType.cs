namespace SnmpConverter;

/// <summary>
/// SNMP PDU type of request
/// </summary>
public enum SnmpPduType : byte
{
    /// <summary>
    /// SNMP Get PDU request
    /// </summary>
    GetRequest = 0xA0,

    /// <summary>
    /// SNMP GetNext PDU request
    /// </summary>
    GetNextRequest = 0xA1,

    /// <summary>
    /// SNMP Response PDU request
    /// </summary>
    Response = 0xA2,

    /// <summary>
    /// SNMP Set PDU request
    /// </summary>
    SetRequest = 0xA3,

    /// <summary>
    /// SNMP Trap PDU request
    /// </summary>
    Trap = 0xA4,

    /// <summary>
    /// SNMP GetBulk PDU request
    /// </summary>
    GetBulkRequest = 0xA5,

    /// <summary>
    /// SNMP Inform PDU request
    /// </summary>
    InformRequest = 0xA6,

    /// <summary>
    /// SNMP Trap v2 PDU request
    /// </summary>
    TrapV2 = 0xA7,

    /// <summary>
    /// SNMP Report PDU request
    /// </summary>
    Report = 0xA8
}