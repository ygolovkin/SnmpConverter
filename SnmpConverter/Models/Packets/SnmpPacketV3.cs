namespace SnmpConverter;

/// <summary>
/// SNMP packet version 3.
/// </summary>
public class SnmpPacketV3 : SnmpBasePacket
{
    /// <summary>
    /// SNMP message identifier.
    /// </summary>
    public int MessageId { get; set; }

    /// <summary>
    /// SNMP message maximum size.
    /// </summary>
    public int MessageMaxSize { get; set; }

    /// <summary>
    /// SNMP message <see cref="SnmpMessageFlag">flag</see>.
    /// </summary>
    public SnmpMessageFlag? MessageFlag { get; set; }

    /// <summary>
    /// SNMP message security model.
    /// </summary>
    public int MessageSecurityModel { get; set; }

    /// <summary>
    /// SNMP <see cref="SnmpEngineId">engine identifier</see>.
    /// </summary>
    public SnmpEngineId? EngineId { get; set; }

    /// <summary>
    /// SNMP engine boots.
    /// </summary>
    public int EngineBoots { get; set; }

    /// <summary>
    /// SNMP engine time.
    /// </summary>
    public int EngineTime { get; set; }

    /// <summary>
    /// SNMP <see cref="SnmpUser">user</see>.
    /// </summary>
    public SnmpUser? User { get; set; }

    /// <summary>
    /// SNMP authentication parameter.
    /// </summary>
    public byte[]? AuthenticationParameter { get; set; }

    /// <summary>
    /// SNMP privacy parameter.
    /// </summary>
    public byte[]? PrivacyParameter { get; set; }

    /// <summary>
    /// SNMP context <see cref="SnmpEngineId">engine identifier</see>.
    /// </summary>
    public SnmpEngineId? ContextEngineId { get; set; }

    /// <summary>
    /// SNMP context name.
    /// </summary>
    public string? ContextName { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SnmpPacketV3"/> class.
    /// </summary>
    public SnmpPacketV3() : base()
    {
        Version = SnmpVersion.V3;
    }
}