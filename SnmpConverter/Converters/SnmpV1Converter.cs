using System.Linq;

namespace SnmpConverter;

internal static class SnmpV1Converter
{
    internal static SnmpPacketV1 SerializeV1(this byte[] source, int offset)
    {
        var communityResult = source.ToCommunity(ref offset);

        var packet = source.SerializeBase<SnmpPacketV1>(offset);
        packet.Community = communityResult;

        return packet;
    }

    internal static byte[] SerializeV1(this SnmpPacketV1 packet)
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
