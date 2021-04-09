using Snmp.Model;
using Snmp.Model.Enums;

namespace Snmp.Serializer
{
    internal static class ErrorStatusExtensions
    {
        internal static SnmpResult<SnmpErrorStatus> GetErrorStatus(this byte[] source, ref int offset)
        {
            var intResult = source.GetInt(ref offset);
            if (intResult.HasError) return new SnmpResult<SnmpErrorStatus>(intResult.Error);
            if (intResult.Value < 0 || intResult.Value > 18) return new SnmpResult<SnmpErrorStatus>("Incorrect value of error status");

            return new SnmpResult<SnmpErrorStatus>((SnmpErrorStatus) intResult.Value);
        }

        internal static SnmpResult<byte[]> ToByteArray(this SnmpErrorStatus source)
        {
            var intResult = ((int)source).ToByteArray();
            return intResult.HasError ? new SnmpResult<byte[]>(intResult.Error) : intResult;
        }
    }
}
