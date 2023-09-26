namespace SnmpConverter;

/// <summary>
/// SNMP packet version 2u.
/// </summary>
public class SnmpPacketV2U : SnmpBasePacket
{
    /// <summary>
    /// SNMP community.
    /// </summary>
    public string? Community { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SnmpPacketV2U"/> class.
    /// </summary>
    public SnmpPacketV2U() : base()
    {
        Version = SnmpVersion.V2U;
    }
}