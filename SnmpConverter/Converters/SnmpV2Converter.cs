using System;

namespace SnmpConverter;

internal static class SnmpV2Converter
{
    internal static SnmpPacketV2C SerializeV2c(this byte[] source, int offset)
    {
        var communityResult = source.ToString(ref offset);

        var typeRequestResult = source.ToPduType(ref offset);

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
            PduType = typeRequestResult.Value,
            RequestId = requestIdResult.Value,
            ErrorStatus = errorStatusResult.Value,
            ErrorIndex = errorIndexResult.Value,
            VariableBindings = variableBindingsResult.Value
        };
        return packet;
    }

    internal static byte[] SerializeV2c(this SnmpPacketV2C packet)
    {
        throw new NotImplementedException();
    }
}