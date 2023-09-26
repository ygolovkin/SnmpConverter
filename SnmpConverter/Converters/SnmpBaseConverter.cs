using System.Linq;

namespace SnmpConverter;

internal static class SnmpBaseConverter
{
    internal static T SerializeBase<T>(this byte[] source, int offset) where T : SnmpBasePacket, new()
    {
        var pduType = source.ToPduType(ref offset);

        source.ToLength(ref offset, x => x < 0, "Array too short.");

        var requestId = source.ToRequestId(ref offset);

        var errorStatus = source.ToErrorStatus(ref offset);

        var errorIndex = source.ToErrorIndex(ref offset);

        var variableBindings = source.ToVariableBindings(ref offset);

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
        var variableBindings = packet.VariableBindings.ToVariableBindingsArray();

        var errorIndex = packet.ErrorIndex.ToErrorIndexArray();

        var errorStatus = packet.ErrorStatus.ToErrorStatusArray();

        var requestIdValue = packet.RequestId.ToRequestIdArray();

        var messageData = requestIdValue
            .Concat(errorStatus)
            .Concat(errorIndex)
            .Concat(variableBindings)
            .ToArray()
            .ToArrayWithLength();

        var pduTypeResult = packet.PduType.ToPduTypeArray();

        return pduTypeResult.Concat(messageData).ToArray();
    }
}
