using System;
using System.Linq;

namespace SnmpConverter;

internal static class SnmpV3Converter
{
    internal static SnmpPacketV3 SerializeV3(this byte[] source, int offset)
    {
        source.ToLength(ref offset, SnmpValueType.CaptionOid, x => x < 5, "Incorrect global data's length.");

        var messageIdResult = source.ToMessageId(ref offset);

        var messageMaxSizeResult = source.ToMessageMaxSize(ref offset);

        var messageFlagResult = source.ToMessageFlag(ref offset);

        var messageSecurityModelResult = source.ToMessageSecurityModel(ref offset);
        
        source.ToLength(ref offset, SnmpValueType.OctetString, x => x < 5, "Incorrect global data's length.");
        
        source.ToLength(ref offset, SnmpValueType.CaptionOid, x => x < 5, "Incorrect global data's length.");

        var engineIdResult = source.ToEngineId(ref offset);

        var engineBootsResult = source.ToEngineBoots(ref offset);

        var engineTimeResult = source.ToEngineTime(ref offset);

        var usernameResult = source.ToUsername(ref offset);

        var userResult = SnmpUsers.Get(usernameResult.Value);
        userResult.HandleError();
        var user = userResult.Value;

        var authenticationParameterResult =
            source.ToAuthenticationParameter(engineIdResult.Value, user, ref offset);

        var privacyParameterResult = source.ToPrivacyParameter(
            engineIdResult.Value, 
            user,
            engineBootsResult.Value, 
            engineTimeResult.Value, 
            ref offset, 
            out var outSource);
        source = outSource;

        source.ToLength(ref offset, SnmpValueType.CaptionOid, x => x < 5, "Incorrect global data's length.");

        var contextEngineIdResult = source.ToContextEngineId(ref offset);
        contextEngineIdResult.HandleError(engineId => !engineIdResult.Value.IsEmpty && !engineId.Equals(user.ContextEngineId), 
            "Incorrect user's Context EngineId.");

        var contextNameResult = source.ToContextName(ref offset);
        contextNameResult.HandleError(x => !engineIdResult.Value.IsEmpty && x != user.ContextName,
            "Incorrect user's Context Name.");

        var packet = source.SerializeBase<SnmpPacketV3>(offset);
        packet.MessageId = messageIdResult.Value;
        packet.MessageMaxSize = messageMaxSizeResult.Value;
        packet.MessageFlag = messageFlagResult.Value;
        packet.MessageSecurityModel = messageSecurityModelResult.Value;
        packet.EngineId = engineIdResult.Value;
        packet.EngineBoots = engineBootsResult.Value;
        packet.EngineTime = engineTimeResult.Value;
        packet.User = userResult.Value;
        packet.AuthenticationParameter = authenticationParameterResult.Value;
        packet.PrivacyParameter = privacyParameterResult.Value;
        packet.ContextEngineId = contextEngineIdResult.Value;
        packet.ContextName = contextNameResult.Value;

        return packet;
    }

    internal static byte[] SerializeV3(this SnmpPacketV3 packet)
    {
        CheckPacket(packet);

        var baseData = packet.SerializeBase();

        var contextNameResult = packet.ContextName.ToByteArray();

        var contextEngineIdResult = packet.ContextEngineId.ToByteArray();

        var messageDataResult = contextEngineIdResult.Value
            .Concat(contextNameResult.Value)
            .Concat(baseData)
            .ToArray()
            .ToLength(SnmpValueType.CaptionOid);

        var privacyParameterResult = messageDataResult.Value.ToByteArray(
            packet.User!,
            packet.EngineBoots,
            packet.EngineTime,
            out var encryptedMessageData);

        packet.AuthenticationParameter = new byte[12];
        var authenticationParameterResult = packet.AuthenticationParameter.ToByteArray();
        var hashBackOffset = encryptedMessageData.Length + packet.AuthenticationParameter.Length;

        var usernameResult = packet.User!.Username.ToByteArray();

        var engineTimeResult = packet.EngineTime.ToByteArray();
        engineTimeResult.HandleError();

        var engineBootsResult = packet.EngineBoots.ToByteArray();
        engineBootsResult.HandleError();

        var engineIdResult = packet.EngineId.ToByteArray();

        var messageAuthoritativeDataResult = engineIdResult.Value 
            .Concat(engineBootsResult.Value) 
            .Concat(engineTimeResult.Value) 
            .Concat(usernameResult.Value) 
            .Concat(authenticationParameterResult.Value) 
            .Concat(privacyParameterResult.Value)
            .ToArray()
            .ToLength(SnmpValueType.CaptionOid).Value
            .ToLength(SnmpValueType.OctetString);

        var messageSecurityModelResult = packet.MessageSecurityModel.ToByteArray();
        messageSecurityModelResult.HandleError();

        var messageFlagResult = packet.MessageFlag!.ToByteArray();

        var messageMaxSizeResult = packet.MessageMaxSize.ToByteArray();
        messageMaxSizeResult.HandleError();

        var messageIdResult = packet.MessageId.ToByteArray();
        messageIdResult.HandleError();

        var messageGlobalDataResult = messageIdResult.Value
            .Concat(messageMaxSizeResult.Value)
            .Concat(messageFlagResult.Value)
            .Concat(messageSecurityModelResult.Value)
            .ToArray()
            .ToLength(SnmpValueType.CaptionOid);

        var versionResult = packet.Version.ToByteArray();
        versionResult.HandleError();

        var buffer = versionResult.Value
            .Concat(messageGlobalDataResult.Value)
            .Concat(messageAuthoritativeDataResult.Value)
            .Concat(encryptedMessageData)
            .ToArray()
            .ToLength(SnmpValueType.CaptionOid)
            .Value;

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