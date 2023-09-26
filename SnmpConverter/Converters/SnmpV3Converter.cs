using System;
using System.Collections.Generic;
using System.Linq;

namespace SnmpConverter;

internal static class SnmpV3Converter
{
    internal static SnmpPacketV3 SerializeV3(this byte[] source, int offset, List<SnmpUser> users)
    {
        source.ToLength(ref offset, SnmpConstants.Sequence, x => x < 5, "Incorrect global data's length.");

        var messageId = source.ToMessageId(ref offset);

        var messageMaxSize = source.ToMessageMaxSize(ref offset);

        var messageFlag = source.ToMessageFlag(ref offset);

        var messageSecurityModel = source.ToMessageSecurityModel(ref offset);
        
        source.ToLength(ref offset, SnmpValueType.OctetString, x => x < 5, "Incorrect global data's length.");
        
        source.ToLength(ref offset, SnmpConstants.Sequence, x => x < 5, "Incorrect global data's length.");

        var engineId = source.ToEngineId(ref offset);

        var engineBoots = source.ToEngineBoots(ref offset);

        var engineTime = source.ToEngineTime(ref offset);

        var username = source.ToUsername(ref offset);

        var user = users.HandleUsername(username);

        var authenticationParameter = source.ToAuthenticationParameter(engineId, user, ref offset);

        var privacyParameter = source.ToPrivacyParameter(
            engineId, 
            user,
            engineBoots, 
            engineTime, 
            ref offset, 
            out var outSource);
        source = outSource;

        source.ToLength(ref offset, SnmpConstants.Sequence, x => x < 5, "Incorrect global data's length.");

        var contextEngineId = source.ToContextEngineId(user, ref offset);

        var contextName = source.ToContextName(engineId, user, ref offset);

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

        var contextName = packet.ContextName.ToContextNameArray();

        var contextEngineId = packet.ContextEngineId.ToEngineIdArray();

        var messageData = contextEngineId
            .Concat(contextName)
            .Concat(baseData)
            .ToArray()
            .ToArrayWithLength(SnmpConstants.Sequence);

        var privacyParameter = messageData.ToPrivacyParameterArray(
            packet.User!,
            packet.EngineBoots,
            packet.EngineTime,
            out var encryptedMessageData);

        packet.AuthenticationParameter = new byte[12];
        var authenticationParameter = packet.AuthenticationParameter.ToAuthenticationParameterArray();
        var hashBackOffset = encryptedMessageData.Length + packet.AuthenticationParameter.Length;

        var username = packet.User!.Name.ToUsernameArray();

        var engineTime = packet.EngineTime.ToEngineTimeArray();

        var engineBoots = packet.EngineBoots.ToEngineBootsArray();

        var engineId = packet.EngineId.ToEngineIdArray();

        var messageAuthoritativeData = engineId 
            .Concat(engineBoots) 
            .Concat(engineTime) 
            .Concat(username) 
            .Concat(authenticationParameter) 
            .Concat(privacyParameter)
            .ToArray()
            .ToArrayWithLength(SnmpConstants.Sequence)
            .ToArrayWithLength(SnmpValueType.OctetString);

        var messageSecurityModel = packet.MessageSecurityModel.ToMessageSecurityModelArray();

        var messageFlag = packet.MessageFlag!.ToMessageFlagArray();

        var messageMaxSize = packet.MessageMaxSize.ToMessageMaxSizeArray();

        var messageId = packet.MessageId.ToMessageIdArray();

        var messageGlobalData = messageId
            .Concat(messageMaxSize)
            .Concat(messageFlag)
            .Concat(messageSecurityModel)
            .ToArray()
            .ToArrayWithLength(SnmpConstants.Sequence);

        var version = packet.Version.ToVersionArray();

        var buffer = version
            .Concat(messageGlobalData)
            .Concat(messageAuthoritativeData)
            .Concat(encryptedMessageData)
            .ToArray()
            .ToArrayWithLength(SnmpConstants.Sequence);

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

        packet.User.CheckUser();

        if(packet.MessageFlag is null)
        {
            throw new SnmpException("Incorrect message flag.", new ArgumentException(nameof(packet.MessageFlag)));
        }
    }
}