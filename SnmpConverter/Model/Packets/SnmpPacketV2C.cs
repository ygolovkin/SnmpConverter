namespace SnmpConverter
{
    public class SnmpPacketV2C : SnmpBasePacket
    {
        public string? Community { get; set; }

        public SnmpPacketV2C() : base()
        {
            Version = SnmpVersion.V2C;
        }
    }
}