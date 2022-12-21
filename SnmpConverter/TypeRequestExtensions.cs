namespace SnmpConverter;

internal static class TypeRequestExtensions
{
    internal static SnmpResult<SnmpTypeRequest> GetTypeRequest(this byte[] source, ref int offset)
    {
        var typeRequest = source[offset++];
        if (typeRequest is < 0 or > 6)
        {
            return new SnmpResult<SnmpTypeRequest>("Incorrect value of type request");
        }
            
        return new SnmpResult<SnmpTypeRequest>((SnmpTypeRequest)typeRequest);
    }

    internal static SnmpResult<byte[]> ToByteArray(this SnmpTypeRequest source)
    {
        return new SnmpResult<byte[]>(new[] {(byte) source});
    }
}