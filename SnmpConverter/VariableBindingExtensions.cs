namespace SnmpConverter;

internal static class VariableBindingExtensions
{
    internal static SnmpResult<VariableBinding> ToVariableBinding(this byte[] source, ref int offset)
    {
        var unwrapResult = source.ToLength(ref offset, SnmpValueType.ObjectIdentifier);
        if (unwrapResult.HasError)
        {
            return new SnmpResult<VariableBinding>(unwrapResult.Error);
        }

        var oidResult = source.GetOid(ref offset);
        if (oidResult.HasError)
        {
            return new SnmpResult<VariableBinding>(oidResult.Error);
        }

        var valueTypeResult = source.GetValueType(ref offset);
        if (valueTypeResult.HasError)
        {
            return new SnmpResult<VariableBinding>(valueTypeResult.Error);
        }

        var valueResult = source.ToValue(ref offset);
        if (valueResult.HasError)
        {
            return new SnmpResult<VariableBinding>(valueResult.Error);
        }

        var variableBinding = new VariableBinding
        {
            Oid = oidResult.Value,
            Type = valueTypeResult.Value,
            Value = valueResult.Value
        };
        return new SnmpResult<VariableBinding>(variableBinding);
    }
}