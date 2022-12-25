using System;
using System.Linq;

namespace SnmpConverter;

internal static class VariableBindingExtensions
{
    internal static SnmpResult<VariableBinding> ToVariableBinding(this byte[] source, ref int offset)
    {
        source.ToLength(ref offset, SnmpValueType.CaptionOid, x => x < 0, "Incorrect variable binding's length.");

        var oidResult = source.ToOid(ref offset);

        var valueTypeResult = source.ToValueType(ref offset);
        valueTypeResult.HandleError();

        var valueResult = source.ToValue(ref offset);
        valueResult.HandleError();

        var variableBinding = new VariableBinding
        {
            Oid = oidResult.Value,
            Type = valueTypeResult.Value,
            Value = valueResult.Value
        };
        return new SnmpResult<VariableBinding>(variableBinding);
    }

    internal static SnmpResult<byte[]> ToByteArray(this VariableBinding? variableBinding)
    {
        if (variableBinding?.Oid is null)
        {
            return new SnmpResult<byte[]>("Incorrect format of variable binding.");
        }

        var oidResult = variableBinding.Oid.ToByteArray();
        oidResult.HandleError();

        var typeResult = variableBinding.Type.ToByteArray();
        typeResult.HandleError();

        var valueResult = variableBinding.Value is null
            ? Array.Empty<byte>().ToByteArray()
            : variableBinding.Value.ToByteArray();
        valueResult.HandleError();

        return new SnmpResult<byte[]>(oidResult.Value.Concat(typeResult.Value).Concat(valueResult.Value).ToArray());
    }
}