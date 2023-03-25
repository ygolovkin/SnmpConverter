﻿using System;

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
                _ => throw new SnmpException("Incorrect", new ArgumentOutOfRangeException(nameof(user.PrivacyType)))
            };
            
            decryptResult.HandleError();
            outSource = decryptResult.Value;
            offset = 0;
        }
        return new SnmpResult<byte[]>(privacyParameters);
    }

    internal static SnmpResult<byte[]> ToByteArray(
        this byte[] source, 
        SnmpUser user,
        int engineBoots,
        int engineTime,
        out byte[] outSource)
    {
        outSource = source;
        if(user is null || user.AuthenticationType == SnmpAuthenticationType.None)
        {
            return new SnmpResult<byte[]>(Array.Empty<byte>());
        }

        if(user.HashKey is null)
        {
            throw new SnmpException("Incorrect hash key.", new ArgumentException(nameof(user.HashKey)));
        }

        byte[] privacyParameters;
        var outSourceResult = user.PrivacyType switch
        {
            SnmpPrivacyType.Des => source.EncryptByDes(user.HashKey, engineBoots, out privacyParameters),
            SnmpPrivacyType.TripleDes => source.EncryptBy3Des(user.HashKey, engineBoots, user.AuthenticationType, out privacyParameters),
            SnmpPrivacyType.Aes128 => source.EncryptByAes128(user.HashKey, engineBoots, engineTime, out privacyParameters),
            SnmpPrivacyType.Aes192 => source.EncryptByAes192(user.HashKey, engineBoots, engineTime, out privacyParameters),
            SnmpPrivacyType.Aes256 => source.EncryptByAes256(user.HashKey, engineBoots, engineTime, out privacyParameters),
            _ => throw new SnmpException("Incorrect privacy type", new ArgumentException(nameof(user.PrivacyType)))
        };

        outSourceResult.HandleError();
        outSource = outSourceResult.Value;
        return new SnmpResult<byte[]>(privacyParameters);
    }
}