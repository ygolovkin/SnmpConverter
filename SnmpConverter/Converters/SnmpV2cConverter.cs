using System.Linq;

namespace SnmpConverter;

internal static class SnmpV2cConverter
{
    internal static SnmpPacketV2C SerializeV2c(this byte[] source, int offset)
    {
        var communityResult = source.ToString(ref offset);

        var pduTypeResult = source.ToPduType(ref offset);

        source.ToLength(ref offset, x => x < 0, "Array too short.");

        var requestIdResult = source.ToRequestId(ref offset);

        var errorStatusResult = source.ToErrorStatus(ref offset);
        errorStatusResult.HandleError();

        var errorIndexResult = source.ToErrorIndex(ref offset);

        var variableBindingsResult = source.ToVariableBindings(ref offset);
        variableBindingsResult.HandleError();

        var packet = new SnmpPacketV2C
        {
            Community = communityResult.Value,
            PduType = pduTypeResult.Value,
            RequestId = requestIdResult.Value,
            ErrorStatus = errorStatusResult.Value,
            ErrorIndex = errorIndexResult.Value,
            VariableBindings = variableBindingsResult.Value
        };
        return packet;
    }

    internal static byte[] SerializeV2c(this SnmpPacketV2C packet)
    {
        var variableBindingsResult = packet.VariableBindings.ToByteArray();
        variableBindingsResult.HandleError();

        var errorIndexValue = packet.ErrorIndex.ToByteArray();
        errorIndexValue.HandleError();

        var errorStatusValue = packet.ErrorStatus.ToByteArray();
        errorStatusValue.HandleError();

        var requestIdValue = packet.RequestId.ToByteArray();
        requestIdValue.HandleError();

        var messageData = requestIdValue.Value
            .Concat(errorStatusValue.Value)
            .Concat(errorIndexValue.Value)
            .Concat(variableBindingsResult.Value)
            .ToArray();

        var messageDataLength = messageData.Length.ToLength().Value;

        var pduTypeResult = packet.PduType.ToByteArray();
        pduTypeResult.HandleError();

        var communityResult = packet.Community.ToByteArray();
        communityResult.HandleError();

        var versionResult = packet.Version.ToByteArray();
        versionResult.HandleError();

        var message = versionResult.Value
            .Concat(communityResult.Value)
            .Concat(pduTypeResult.Value)
            .Concat(messageDataLength)
            .Concat(messageData)
            .ToArray();

        var result = message.ToLength(SnmpValueType.CaptionOid);
        result.HandleError();
        return result.Value;
    }
}