using Snmp.Model;
using Snmp.Model.Enums;
using Snmp.Model.Packet;

namespace Snmp.Serializer
{
    internal static class VariableBindingExtensions
    {
        internal static SnmpResult<VariableBiniding> GetVariableBiniding(this byte[] source, ref int offset)
        {
            if (source[offset++] != (byte)SnmpValueType.Object_Identifier) return new SnmpResult<VariableBiniding>("Incorrect type of variable biniding");

            var lengthResult = source.GetLength(ref offset);
            if (lengthResult.HasError) return new SnmpResult<VariableBiniding>(lengthResult.Error);

            var variableBiniding = new VariableBiniding();

            var oidResult = source.GetOid(ref offset);
            if (oidResult.HasError) return new SnmpResult<VariableBiniding>(oidResult.Error);
            variableBiniding.Oid = oidResult.Value;

            var valueTypeResult = source.GetValueType(ref offset);
            if (valueTypeResult.HasError) return new SnmpResult<VariableBiniding>(valueTypeResult.Error);
            variableBiniding.Type = valueTypeResult.Value;

            var valueResult = source.GetValue(ref offset);
            if (valueResult.HasError) return new SnmpResult<VariableBiniding>(valueResult.Error);
            variableBiniding.Value = valueResult.Value;

            return new SnmpResult<VariableBiniding>(variableBiniding);
        }

    }
}