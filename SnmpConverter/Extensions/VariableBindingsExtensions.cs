using System.Collections.Generic;

namespace SnmpConverter;

internal static class VariableBindingsExtensions
{
    internal static ICollection<SnmpVariableBinding> ToVariableBindings(this byte[] source, ref int offset)
    {
        source.ToLength(ref offset, SnmpConstants.Sequence, x => x < 0, "Incorrect variable bindings length.");

        var variableBindings = new List<SnmpVariableBinding>();
        while (offset != source.Length)
        {
            if (offset > source.Length)
            {
               throw new SnmpException("Incorrect variable bindings format.");
            }

            var variableBinding = source.ToVariableBinding(ref offset);
            variableBindings.Add(variableBinding);
        }

        return variableBindings;
    }

    internal static byte[] ToVariableBindingsArray(this ICollection<SnmpVariableBinding>? variableBindings)
    {
        if (variableBindings is null)
        {
            throw new SnmpException("Incorrect format of variable bindings.");
        }

        var result = new List<byte>();
        foreach (var variableBinding in variableBindings)
        {
            result.AddRange(variableBinding.ToVariableBindingArray());
        }

        return result.ToArray().ToArrayWithLength(SnmpConstants.Sequence);
    }
}