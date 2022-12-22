using System.Collections.Generic;

namespace SnmpConverter;

internal static class VariableBindingsExtensions
{
    internal static SnmpResult<ICollection<VariableBinding>> ToVariableBindings(this byte[] source, ref int offset)
    {
        source.ToLength(ref offset, SnmpValueType.ObjectIdentifier);

        var variableBindings = new List<VariableBinding>();
        while (offset != source.Length)
        {
            if (offset > source.Length)
            {
                return new SnmpResult<ICollection<VariableBinding>>("Incorrect packet format");
            }

            var variableBindingResult = source.ToVariableBinding(ref offset);
            if (variableBindingResult.HasError)
            {
                return new SnmpResult<ICollection<VariableBinding>>(variableBindingResult.Error);
            }
            variableBindings.Add(variableBindingResult.Value);
        }

        return new SnmpResult<ICollection<VariableBinding>>(variableBindings);
    }
}