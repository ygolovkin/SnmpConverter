using System;
using System.Linq;

namespace SnmpConverter;

internal static class VariableBindingExtensions
{
    internal static SnmpResult<SnmpVariableBinding> ToVariableBinding(this byte[] source, ref int offset)
    {
        source.ToLength(ref offset, SnmpConstants.Sequence)
            .HandleError(x => x < 0, "Incorrect variable binding's length.");

        var oid = source.ToOid(ref offset).HandleError();

        var valueType = source.ToValueType(ref offset).HandleError();

        var value = source.ToValue(ref offset).HandleError();

        var variableBinding = new SnmpVariableBinding
        {
            Oid = oid,
            Type = valueType,
            Value = value
        };
        return new SnmpResult<SnmpVariableBinding>(variableBinding);
    }

    internal static SnmpResult<byte[]> ToByteArray(this SnmpVariableBinding? variableBinding)
    {
        if (variableBinding?.Oid is null)
        {
            return new SnmpResult<byte[]>("Incorrect format of variable binding.");
        }

        var oid = variableBinding.Oid.ToByteArray().HandleError();

        var valueType = variableBinding.Type.ToByteArray().HandleError();

        var value = variableBinding.Value.ToByteArray().HandleError();

        return new SnmpResult<byte[]>(oid.Concat(valueType).Concat(value).ToArray());
    }
}