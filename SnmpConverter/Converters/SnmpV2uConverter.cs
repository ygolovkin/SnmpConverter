using System.Linq;

namespace SnmpConverter;

internal static class SnmpV2uConverter
{
    internal static SnmpPacketV2U SerializeV2u(this byte[] source, int offset)
    {
        var community = source.ToCommunity(ref offset);

        var packet = source.SerializeBase<SnmpPacketV2U>(offset);
        packet.Community = community;

        return packet;
    }

    internal static byte[] SerializeV2u(this SnmpPacketV2U packet)
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