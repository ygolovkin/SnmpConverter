using System.Collections.Generic;

namespace SnmpConverter;

internal static class VariableBindingsExtensions
{
    internal static SnmpResult<ICollection<SnmpVariableBinding>> ToVariableBindings(this byte[] source, ref int offset)
    {
        source.ToLength(ref offset, SnmpValueType.CaptionOid, x => x < 0, "Incorrect variable bindings length.");

        var variableBindings = new List<SnmpVariableBinding>();
        while (offset != source.Length)
        {
            if (offset > source.Length)
            {
                return new SnmpResult<ICollection<SnmpVariableBinding>>("Incorrect variable bindings format");
            }

            var variableBindingResult = source.ToVariableBinding(ref offset);
            if (variableBindingResult.HasError)
            {
                return new SnmpResult<ICollection<SnmpVariableBinding>>(variableBindingResult.Error);
            }
            variableBindings.Add(variableBindingResult.Value);
        }

        return new SnmpResult<ICollection<SnmpVariableBinding>>(variableBindings);
    }

    internal static SnmpResult<byte[]> ToByteArray(this ICollection<SnmpVariableBinding>? variableBindings)
    {
        if (variableBindings is null)
        {
            return new SnmpResult<byte[]>("Incorrect format of variable bindings.");
        }

        var result = new List<byte>();
        foreach (var variableBinding in variableBindings)
        {
            var variableBindingValue = variableBinding.ToByteArray();
            variableBindingValue.HandleError();
            result.AddRange(variableBindingValue.Value);
        }

        return result.ToArray().ToLength(SnmpValueType.CaptionOid);
    }
}