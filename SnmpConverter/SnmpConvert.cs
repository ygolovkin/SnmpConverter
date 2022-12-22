using System;

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

        if (length < 0 || length > source.Length - offset)
        {
            throw new SnmpException("Incorrect format", new ArgumentException(null, nameof(length)));
        }

        var buffer = new byte[length];
        Buffer.BlockCopy(source, offset, buffer, 0, length);

        if (buffer[offset++] != (byte)SnmpValueType.CaptionOid)
        {
            throw new SnmpException("Incorrect format");
        }

        buffer.ToLength(ref offset, new SnmpHandlerError<int>(len => len < 2, "Array too short"));

        var versionResult = buffer.ToVersion(ref offset);

        return versionResult.Value switch
        {
            SnmpVersion.V2C => SerializeV2c(buffer, offset),
            _ => throw new SnmpException("Unsupported version")
        };
    }

    private static SnmpPacketV2C SerializeV2c(this byte[] source, int offset)
    {
        var communityResult = source.ToString(ref offset);

        var typeRequestResult = source.GetTypeRequest(ref offset);

        source.ToLength(ref offset);

        var requestIdResult = source.ToRequestId(ref offset);

        var errorStatusResult = source.ToErrorStatus(ref offset);

        var errorIndexResult = source.ToErrorIndex(ref offset);

        var variableBindingsResult = source.ToVariableBindings(ref offset);
        variableBindingsResult.HandleError();

        var packet = new SnmpPacketV2C
        {
            Community = communityResult.Value,
            TypeRequest = typeRequestResult.Value,
            RequestId = requestIdResult.Value,
            ErrorStatus = errorStatusResult.Value,
            ErrorIndex = errorIndexResult.Value,
            VariableBindings = variableBindingsResult.Value
        };
        return packet;
    }


    public static byte[] Serialize(this SnmpBasePacket? packet)
    {
        var result = packet.IsPacketCorrect();
        result.HandleError();

        return packet switch
        {
            SnmpPacketV2C v2C => v2C.SerializeV2c(),
            _ => throw new SnmpException("Unsupported version")
        };
    }
        
    private static byte[] SerializeV2c(this SnmpPacketV2C packet)
    {
        throw new NotImplementedException();
    }



    private static SnmpResult<bool> IsPacketCorrect(this SnmpBasePacket? packet)
    {
        if (packet is null)
        {
            return new SnmpResult<bool>("Packet cannot be null");
        }
            
        return new SnmpResult<bool>(true);
    }
}