using System.Linq;

namespace SnmpConverter;

internal static class SnmpV2cConverter
{
    internal static SnmpPacketV2C SerializeV2c(this byte[] source, int offset)
    {
        var community = source.ToString(ref offset).HandleError();

        var packet = source.SerializeBase<SnmpPacketV2C>(offset);
        packet.Community = community;

        return packet;
    }

    internal static byte[] SerializeV2c(this SnmpPacketV2C packet)
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