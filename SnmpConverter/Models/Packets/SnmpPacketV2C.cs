namespace SnmpConverter;

/// <summary>
/// SNMP packet version 2c.
/// </summary>
public class SnmpPacketV2C : SnmpBasePacket
{
    /// <summary>
    /// SNMP community.
    /// </summary>
    public string? Community { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SnmpPacketV2C"/> class.
    /// </summary>
    public SnmpPacketV2C() : base()
    {
        Version = SnmpVersion.V2C;
    }
}