namespace SnmpConverter;

public class SnmpPacketV1 : SnmpBasePacket
{
    public string? Community { get; set; }

    public SnmpPacketV1()
    {
        Version = SnmpVersion.V1;
    }
}
