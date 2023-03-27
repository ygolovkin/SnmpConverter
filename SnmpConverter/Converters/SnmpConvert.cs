﻿using System;

namespace SnmpConverter;

public static class SnmpConvert
{
    public static SnmpBasePacket Serialize(this byte[]? source)
    {
        if (source is null)
        {
            throw new SnmpException("Incorrect format", new ArgumentNullException(nameof(source)));
        }

        return source.Serialize(0, source.Length);
    }

    public static SnmpBasePacket Serialize(this byte[]? source, int offset)
    {
        if (source is null)
        {
            throw new SnmpException("Incorrect format", new ArgumentNullException(nameof(source)));
        }

        return source.Serialize(offset, source.Length - offset);
    }

    public static SnmpBasePacket Serialize(this byte[]? source, int offset, int length)
    {
        if (source is null)
        {
            throw new SnmpException("Incorrect format", new ArgumentNullException(nameof(source)));
        }

        if (source.Length == 0)
        {
            throw new SnmpException("Incorrect format", new ArgumentException(null, nameof(source)));
        }

        if (offset < 0 || offset > source.Length)
        {
            throw new SnmpException("Incorrect format", new ArgumentException(null, nameof(offset)));
        }

        if (length < SnmpConstants.MinBufferSize || length > source.Length - offset)
        {
            throw new SnmpException("Incorrect format", new ArgumentException(null, nameof(length)));
        }

        var buffer = new byte[length];
        Buffer.BlockCopy(source, offset, buffer, 0, length);

        if (buffer[offset++] != SnmpConstants.Sequence)
        {
            throw new SnmpException("Incorrect format");
        }

        buffer.ToLength(ref offset).HandleError(x => x < 2, "Array too short");

        var version = buffer.ToVersion(ref offset).HandleError();

        return version switch
        {
            SnmpVersion.V1 => buffer.SerializeV1(offset),
            SnmpVersion.V2C => buffer.SerializeV2c(offset),
            SnmpVersion.V2U => buffer.SerializeV2u(offset),
            SnmpVersion.V3 => buffer.SerializeV3(offset),
            _ => throw new SnmpException("Unsupported version")
        };
    }

    public static byte[] Serialize(this SnmpBasePacket? packet)
    {
        if (packet is null)
        {
            throw new SnmpException("Incorrect format", new ArgumentNullException(nameof(packet)));
        }

        return packet switch
        {
            SnmpPacketV1 v1 => v1.SerializeV1(),
            SnmpPacketV2C v2c => v2c.SerializeV2c(),
            SnmpPacketV2U v2u => v2u.SerializeV2u(),
            SnmpPacketV3 v3 => v3.SerializeV3(),
            _ => throw new SnmpException("Unsupported version")
        };
    }
}