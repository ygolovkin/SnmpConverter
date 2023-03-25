namespace SnmpConverter;

public class SnmpPacketV3 : SnmpBasePacket
{
    public int MessageId { get; set; }

    public int MessageMaxSize { get; set; }

    public SnmpMessageFlag? MessageFlag { get; set; }

    public int MessageSecurityModel { get; set; }

    public SnmpEngineId? EngineId { get; set; }

    public int EngineBoots { get; set; }

    public int EngineTime { get; set; }

    public SnmpUser? User { get; set; }

    public byte[]? AuthenticationParameter { get; set; }

    public byte[]? PrivacyParameter { get; set; }

    public SnmpEngineId? ContextEngineId { get; set; }

    public string? ContextName { get; set; }

    public SnmpPacketV3() : base()
    {
        Version = SnmpVersion.V3;
    }
}