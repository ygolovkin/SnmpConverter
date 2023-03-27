using System;
using System.Linq;

namespace SnmpConverter;

internal static class SnmpV3Converter
{
    internal static SnmpPacketV3 SerializeV3(this byte[] source, int offset)
    {
        source.ToLength(ref offset, SnmpConstants.Sequence).HandleError(x => x < 5, "Incorrect global data's length.");

        var messageId = source.ToMessageId(ref offset).HandleError();

        var messageMaxSize = source.ToMessageMaxSize(ref offset).HandleError();

        var messageFlag = source.ToMessageFlag(ref offset).HandleError();

        var messageSecurityModel = source.ToMessageSecurityModel(ref offset).HandleError();
        
        source.ToLength(ref offset, SnmpValueType.OctetString).HandleError(x => x < 5, "Incorrect global data's length.");
        
        source.ToLength(ref offset, SnmpConstants.Sequence).HandleError(x => x < 5, "Incorrect global data's length.");

        var engineId = source.ToEngineId(ref offset).HandleError();

        var engineBoots = source.ToEngineBoots(ref offset).HandleError();

        var engineTime = source.ToEngineTime(ref offset).HandleError();

        var username = source.ToUsername(ref offset).HandleError();

        var user = SnmpUsers.Get(username).HandleError();

        var authenticationParameter = source.ToAuthenticationParameter(engineId, user, ref offset).HandleError();

        var privacyParameter = source.ToPrivacyParameter(
            engineId,
            user,
            engineBoots,
            engineTime,
            ref offset,
            out var outSource).HandleError();
        source = outSource;

        source.ToLength(ref offset, SnmpConstants.Sequence).HandleError(x => x < 5, "Incorrect global data's length.");

        var contextEngineId = source.ToContextEngineId(ref offset)
            .HandleError(engineId => !engineId.IsEmpty && !engineId.Equals(user.ContextEngineId),
                "Incorrect user's Context EngineId.");

        var contextName = source.ToContextName(ref offset)
            .HandleError(x => !engineId.IsEmpty && x != user.ContextName,
                "Incorrect user's Context Name.");

        var packet = source.SerializeBase<SnmpPacketV3>(offset);
        packet.MessageId = messageId;
        packet.MessageMaxSize = messageMaxSize;
        packet.MessageFlag = messageFlag;
        packet.MessageSecurityModel = messageSecurityModel;
        packet.EngineId = engineId;
        packet.EngineBoots = engineBoots;
        packet.EngineTime = engineTime;
        packet.User = user;
        packet.AuthenticationParameter = authenticationParameter;
        packet.PrivacyParameter = privacyParameter;
        packet.ContextEngineId = contextEngineId;
        packet.ContextName = contextName;

        return packet;
    }

    internal static byte[] SerializeV3(this SnmpPacketV3 packet)
    {
        CheckPacket(packet);

        var baseData = packet.SerializeBase();

        var contextName = packet.ContextName.ToByteArray().HandleError();

        var contextEngineId = packet.ContextEngineId.ToByteArray().HandleError();

        var messageData = contextEngineId
            .Concat(contextName)
            .Concat(baseData)
            .ToArray()
            .ToLength(SnmpConstants.Sequence)
            .HandleError();

        var privacyParameter = messageData.ToByteArray(
            packet.User!,
            packet.EngineBoots,
            packet.EngineTime,
            out var encryptedMessageData)
            .HandleError();

        packet.AuthenticationParameter = new byte[12];
        var authenticationParameter = packet.AuthenticationParameter.ToByteArray().HandleError();
        var hashBackOffset = encryptedMessageData.Length + packet.AuthenticationParameter.Length;

        var username = packet.User!.Name.ToByteArray().HandleError();

        var engineTime = packet.EngineTime.ToByteArray().HandleError();

        var engineBoots = packet.EngineBoots.ToByteArray().HandleError();

        var engineId = packet.EngineId.ToByteArray().HandleError();

        var messageAuthoritativeData = engineId 
            .Concat(engineBoots) 
            .Concat(engineTime) 
            .Concat(username) 
            .Concat(authenticationParameter) 
            .Concat(privacyParameter)
            .ToArray()
            .ToLength(SnmpConstants.Sequence)
            .HandleError()
            .ToLength(SnmpValueType.OctetString)
            .HandleError();

        var messageSecurityModel = packet.MessageSecurityModel.ToByteArray().HandleError();

        var messageFlag = packet.MessageFlag!.ToByteArray().HandleError();

        var messageMaxSize = packet.MessageMaxSize.ToByteArray().HandleError();

        var messageId = packet.MessageId.ToByteArray().HandleError();

        var messageGlobalData = messageId
            .Concat(messageMaxSize)
            .Concat(messageFlag)
            .Concat(messageSecurityModel)
            .ToArray()
            .ToLength(SnmpConstants.Sequence)
            .HandleError();

        var version = packet.Version.ToByteArray().HandleError();

        var buffer = version
            .Concat(messageGlobalData)
            .Concat(messageAuthoritativeData)
            .Concat(encryptedMessageData)
            .ToArray()
            .ToLength(SnmpConstants.Sequence)
            .HandleError();

        if(packet.User.AuthenticationType != SnmpAuthenticationType.None)
        {
            var hash = buffer.GetHash(packet.User);
            Buffer.BlockCopy(hash, 0, buffer, buffer.Length - hashBackOffset, hash.Length);
        }

        return buffer;
    }

    private static void CheckPacket(SnmpPacketV3 packet)
    {
        if(packet.User is null)
        {
            throw new SnmpException("Incorrect user.", new ArgumentException(nameof(packet.User)));
        }

        if(packet.MessageFlag is null)
        {
            throw new SnmpException("Incorrect message flag.", new ArgumentException(nameof(packet.MessageFlag)));
        }
    }
}