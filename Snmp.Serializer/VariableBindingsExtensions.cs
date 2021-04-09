using System.Collections.Generic;
using Snmp.Model;
using Snmp.Model.Enums;
using Snmp.Model.Packet;

namespace Snmp.Serializer
{
    internal static class VariableBindingsExtensions
    {
        internal static SnmpResult<ICollection<VariableBiniding>> GetVariableBinidings(this byte[] source, ref int offset)
        {
            if (source[offset++] != (byte)SnmpValueType.Object_Identifier) return new SnmpResult<ICollection<VariableBiniding>>("Incorrect type of variable binidings");
            
            var lengthResult = source.GetLength(ref offset);
            if (lengthResult.HasError) return new SnmpResult<ICollection<VariableBiniding>>(lengthResult.Error);

            var variableBinidings = new List<VariableBiniding>();
            while (offset != source.Length)
            {
                var variableBinidingResult = source.GetVariableBiniding(ref offset);
                if (variableBinidingResult.HasError) return new SnmpResult<ICollection<VariableBiniding>>(variableBinidingResult.Error);
                variableBinidings.Add(variableBinidingResult.Value);
            }

            return new SnmpResult<ICollection<VariableBiniding>>(variableBinidings);
        }
    }
}
