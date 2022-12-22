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
}