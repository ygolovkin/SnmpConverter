namespace SnmpConverter;

/// <summary>
/// SNMP packet version 1.
/// </summary>
public class SnmpPacketV1 : SnmpBasePacket
{
    /// <summary>
    /// SNMP community.
    /// </summary>
    public string? Community { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SnmpPacketV1"/> class.
    /// </summary>
    public SnmpPacketV1()
    {
        Version = SnmpVersion.V1;
    }
}
