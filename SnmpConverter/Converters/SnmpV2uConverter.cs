using System.Linq;

namespace SnmpConverter;

internal static class SnmpV2uConverter
{
    internal static SnmpPacketV2U SerializeV2u(this byte[] source, int offset)
    {
        var community = source.ToString(ref offset).HandleError();

        var packet = source.SerializeBase<SnmpPacketV2U>(offset);
        packet.Community = community;

        return packet;
    }

    internal static byte[] SerializeV2u(this SnmpPacketV2U packet)
    {
        var baseData = packet.SerializeBase();

        var community = packet.Community.ToByteArray().HandleError();

        var version = packet.Version.ToByteArray().HandleError();

        return version
            .Concat(community)
            .Concat(baseData)
            .ToArray()
            .ToLength(SnmpConstants.Sequence)
            .HandleError();
    }
}