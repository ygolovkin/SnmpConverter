using System.Linq;

namespace SnmpConverter;

internal static class SnmpV1Converter
{
    internal static SnmpPacketV1 SerializeV1(this byte[] source, int offset)
    {
        var communityResult = source.ToString(ref offset);

        var packet = source.SerializeBase<SnmpPacketV1>(offset);
        packet.Community = communityResult.Value;

        return packet;
    }

    internal static byte[] SerializeV1(this SnmpPacketV1 packet)
    {
        var baseData = packet.SerializeBase();

        var communityResult = packet.Community.ToByteArray();
        communityResult.HandleError();

        var versionResult = packet.Version.ToByteArray();
        versionResult.HandleError();

        return versionResult.Value
            .Concat(communityResult.Value)
            .Concat(baseData)
            .ToArray()
            .ToLength(SnmpConstants.Sequence)
            .Value;
    }
}
