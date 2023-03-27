using System.Linq;

namespace SnmpConverter;

internal static class SnmpV2cConverter
{
    internal static SnmpPacketV2C SerializeV2c(this byte[] source, int offset)
    {
        var communityResult = source.ToString(ref offset);

        var packet = source.SerializeBase<SnmpPacketV2C>(offset);
        packet.Community = communityResult.Value;

        return packet;
    }

    internal static byte[] SerializeV2c(this SnmpPacketV2C packet)
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