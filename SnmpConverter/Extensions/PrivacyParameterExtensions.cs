using System;

namespace SnmpConverter;

internal static class PrivacyParameterExtensions
{
    internal static SnmpResult<byte[]> ToPrivacyParameter(
        this byte[] source, 
        SnmpEngineId engineId, 
        SnmpUser user,
        int engineBoots,
        int engineTime,
        ref int offset, 
        out byte[] outSource)
    {
        outSource = source;
        var length = source.ToLength(ref offset, SnmpValueType.OctetString, x => x is < 5 or > 32,
            "Incorrect authentication parameter's length.").Value;

        var privacyParameters = new byte[length];
        Buffer.BlockCopy(source, offset, privacyParameters, 0, length);
        offset += length;

        if (!engineId.IsEmpty && user.PrivacyType != SnmpPrivacyType.None)
        {
            length = source.ToLength(ref offset, SnmpValueType.OctetString, x => x is < 5 or > 32,
                "Incorrect unpacking encrypt data.").Value;

            var decryptResult = user.PrivacyType switch
            {
                SnmpPrivacyType.Des => source.DecryptByDes(offset, length, user.HashKey!, privacyParameters),
                SnmpPrivacyType.TripleDes => source.DecryptByTripleDes(offset, length, user.HashKey!, privacyParameters),
                SnmpPrivacyType.Aes128 => source.DecryptByAes128(offset, length, user.HashKey!, engineBoots, engineTime, privacyParameters),
                SnmpPrivacyType.Aes192 => source.DecryptByAes192(offset, length, user.HashKey!, engineBoots, engineTime, privacyParameters),
                SnmpPrivacyType.Aes256 => source.DecryptByAes256(offset, length, user.HashKey!, engineBoots, engineTime, privacyParameters),
                _ => throw new ArgumentOutOfRangeException()
            };
            
            decryptResult.HandleError();
            outSource = decryptResult.Value;
            offset = 0;
        }
        return new SnmpResult<byte[]>(privacyParameters);
    }
}