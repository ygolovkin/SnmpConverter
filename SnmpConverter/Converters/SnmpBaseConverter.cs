using System.Linq;

namespace SnmpConverter;

internal static class SnmpBaseConverter
{
    internal static T SerializeBase<T>(this byte[] source, int offset) where T : SnmpBasePacket, new()
    {
        var pduType = source.ToPduType(ref offset).HandleError();

        source.ToLength(ref offset).HandleError(x => x < 0, "Array too short.");

        var requestId = source.ToRequestId(ref offset).HandleError();

        var errorStatus = source.ToErrorStatus(ref offset).HandleError();

        var errorIndex = source.ToErrorIndex(ref offset).HandleError();

        var variableBindings = source.ToVariableBindings(ref offset).HandleError();

        return new T()
        {
            PduType = pduType,
            RequestId = requestId,
            ErrorStatus = errorStatus,
            ErrorIndex = errorIndex,
            VariableBindings = variableBindings
        };
    }

    internal static byte[] SerializeBase(this SnmpBasePacket packet)
    {
        var variableBindings = packet.VariableBindings.ToByteArray().HandleError();

        var errorIndex = packet.ErrorIndex.ToByteArray().HandleError();

        var errorStatus = packet.ErrorStatus.ToByteArray().HandleError();

        var requestId = packet.RequestId.ToByteArray().HandleError();

        var messageData = requestId
            .Concat(errorStatus)
            .Concat(errorIndex)
            .Concat(variableBindings)
            .ToArray()
            .ToLength()
            .HandleError();

        var pduType = packet.PduType.ToByteArray().HandleError();

        return pduType.Concat(messageData).ToArray();
    }
}
