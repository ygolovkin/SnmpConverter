using System.Linq;

namespace SnmpConverter;

internal static class SnmpV2cConverter
{
    internal static SnmpPacketV2C SerializeV2c(this byte[] source, int offset)
    {
        var community = source.ToCommunity(ref offset);

        var packet = source.SerializeBase<SnmpPacketV2C>(offset);
        packet.Community = community;

        return packet;
    }

    internal static byte[] SerializeV2c(this SnmpPacketV2C packet)
    {
        var baseData = packet.SerializeBase();

        var community = packet.Community.ToCommunityArray();

        var version = packet.Version.ToVersionArray();

        return version
            .Concat(community)
            .Concat(baseData)
            .ToArray()
            .ToArrayWithLength(SnmpConstants.Sequence);
    }
}