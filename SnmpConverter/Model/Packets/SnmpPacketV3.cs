using System;

namespace SnmpConverter;

public class SnmpPacketV3 : SnmpBasePacket
{
    public SnmpPacketV3() : base()
    {
        Version = SnmpVersion.V3;
    }
}