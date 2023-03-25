using System.Linq;

namespace SnmpConverter;

internal static class SnmpBaseConverter
{
    internal static T SerializeBase<T>(this byte[] source, int offset) where T : SnmpBasePacket, new()
    {
        var pduTypeResult = source.ToPduType(ref offset);

        source.ToLength(ref offset, x => x < 0, "Array too short.");

        var requestIdResult = source.ToRequestId(ref offset);

        var errorStatusResult = source.ToErrorStatus(ref offset);
        errorStatusResult.HandleError();

        var errorIndexResult = source.ToErrorIndex(ref offset);

        var variableBindingsResult = source.ToVariableBindings(ref offset);
        variableBindingsResult.HandleError();

        return new T()
        {
            PduType = pduTypeResult.Value,
            RequestId = requestIdResult.Value,
            ErrorStatus = errorStatusResult.Value,
            ErrorIndex = errorIndexResult.Value,
            VariableBindings = variableBindingsResult.Value
        };
    }

    internal static byte[] SerializeBase(this SnmpBasePacket packet)
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
            .ToArray()
            .ToLength()
            .Value;

        var pduTypeResult = packet.PduType.ToByteArray();
        pduTypeResult.HandleError();

        return pduTypeResult.Value.Concat(messageData).ToArray();
    }
}
