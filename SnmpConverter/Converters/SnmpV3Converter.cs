using System;

namespace SnmpConverter;

internal static class SnmpV3Converter
{
    internal static SnmpPacketV3 SerializeV3(this byte[] source, int offset)
    {
        source.ToLength(ref offset, SnmpValueType.CaptionOid, x => x < 5, "Incorrect global data's length.");

        var messageIdResult = source.ToMessageId(ref offset);

        var messageMaxSizeResult = source.ToMessageId(ref offset);

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
        contextEngineIdResult.HandleError(x =>
                !engineIdResult.Value.IsEmpty && !user.ContextEngineId!.ToArray().IsEqual(x.ToArray()),
            "Incorrect user's Context EngineId.");

        var contextNameResult = source.ToContextName(ref offset);
        contextNameResult.HandleError(x => !engineIdResult.Value.IsEmpty && x != user.ContextName,
            "Incorrect user's Context Name.");

        var pduTypeResult = source.ToPduType(ref offset);

        source.ToLength(ref offset, x => x < 0, "Array too short.");

        var requestIdResult = source.ToRequestId(ref offset);

        var errorStatusResult = source.ToErrorStatus(ref offset);
        errorStatusResult.HandleError();

        var errorIndexResult = source.ToErrorIndex(ref offset);

        var variableBindingsResult = source.ToVariableBindings(ref offset);
        variableBindingsResult.HandleError();

        var packet = new SnmpPacketV3
        {
            MessageId = messageIdResult.Value,
            MessageMaxSize = messageMaxSizeResult.Value,
            MessageFlag = messageFlagResult.Value,
            MessageSecurityModel = messageSecurityModelResult.Value,
            EngineId = engineIdResult.Value,
            EngineBoots = engineBootsResult.Value,
            EngineTime = engineTimeResult.Value,
            User = userResult.Value,
            AuthenticationParameter = authenticationParameterResult.Value,
            PrivacyParameter = privacyParameterResult.Value,
            ContextEngineId = contextEngineIdResult.Value,
            ContextName = contextNameResult.Value,
            PduType = pduTypeResult.Value,
            RequestId = requestIdResult.Value,
            ErrorStatus = errorStatusResult.Value,
            ErrorIndex = errorIndexResult.Value,
            VariableBindings = variableBindingsResult.Value
        };

        return packet;
    }

    internal static byte[] SerializeV3(this SnmpPacketV2C packet)
    {
        throw new NotImplementedException();
    }
}