using System;
using System.Linq;

namespace SnmpConverter;

internal static class VariableBindingExtensions
{
    internal static SnmpVariableBinding ToVariableBinding(this byte[] source, ref int offset)
    {
        source.ToLength(ref offset, SnmpConstants.Sequence, x => x < 0, "Incorrect variable binding's length.");

        var oid = source.ToOid(ref offset);

        var valueType = source.ToValueType(ref offset);

        var value = source.ToValue(ref offset);

        return new SnmpVariableBinding
        {
            Oid = oid,
            Type = valueType,
            Value = value
        };
    }

    internal static byte[] ToVariableBindingArray(this SnmpVariableBinding? variableBinding)
    {
        if (variableBinding?.Oid is null)
        {
            throw new SnmpException("Incorrect format of variable binding.");
        }

        var oid = variableBinding.Oid.ToOidArray();

        var type = variableBinding.Type.ToValueTypeArray();

        var value = variableBinding.Value.ToValueArray();

        return oid.Concat(type).Concat(value).ToArray();
    }
}