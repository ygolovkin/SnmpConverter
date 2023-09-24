﻿using System.Linq;

namespace SnmpConverter;

internal static class SnmpV1Converter
{
    internal static SnmpPacketV1 SerializeV1(this byte[] source, int offset)
    {
        var community = source.ToString(ref offset).HandleError();

        var packet = source.SerializeBase<SnmpPacketV1>(offset);
        packet.Community = community;

        return packet;
    }

    internal static byte[] SerializeV1(this SnmpPacketV1 packet)
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
