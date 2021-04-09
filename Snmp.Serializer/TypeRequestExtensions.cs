using Snmp.Model;
using Snmp.Model.Enums;

namespace Snmp.Serializer
{
    internal static class TypeRequestExtensions
    {
        internal static SnmpResult<SnmpTypeRequest> GetTypeRequest(this byte[] source, ref int offset)
        {
            var typeRequest = source[offset++];
            if(typeRequest < 0 || typeRequest > 6) return new SnmpResult<SnmpTypeRequest>("Incorrect value of type request");
            
            return new SnmpResult<SnmpTypeRequest>((SnmpTypeRequest)typeRequest);
        }

        internal static SnmpResult<byte[]> ToByteArray(this SnmpTypeRequest source)
        {
            return new SnmpResult<byte[]>(new[] {(byte) source});
        }
    }
}
