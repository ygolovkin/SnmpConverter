﻿using System;
using System.Security.Cryptography;

namespace SnmpConverter;

internal static class DecryptingExtensions
{
    internal static byte[] DecryptByDes(this byte[]? source, int offset, int length, byte[] key, byte[] privacyParameters)
    {
        if (length % 8 != 0
            || source == null
            || source.Length == 0
            || offset > source.Length
            || offset + length > source.Length)
        {
            throw new SnmpException("Incorrect data.");
        }

        var data = new byte[length];
        Buffer.BlockCopy(source, offset, data, 0, length);

        var iv = new byte[8];
        for (var i = 0; i < 8; ++i)
        {
            iv[i] = (byte)(key[8 + i] ^ privacyParameters[i]);
        }

        var des = DES.Create();
        des.Mode = CipherMode.CBC;
        des.Padding = PaddingMode.Zeros;
        var outKey = new byte[8];
        Buffer.BlockCopy(key, 0, outKey, 0, 8);

        des.Key = outKey;
        des.IV = iv;

        var transform = des.CreateDecryptor();
        var decryptedData = transform.TransformFinalBlock(data, 0, data.Length);
        des.Clear();

        return decryptedData;
    }

    internal static byte[] DecryptByTripleDes(this byte[]? source, int offset, int length, byte[] key, byte[] privacyParameters)
    {
        if (length % 8 != 0
            || source == null
            || source.Length == 0
            || offset > source.Length
            || offset + length > source.Length)
        {
            throw new SnmpException("Incorrect data.");
        }

        var iv = new byte[8];
        for (var i = 0; i < iv.Length; i++)
        {
            iv[i] = (byte)(privacyParameters[i] ^ key[24 + i]);
        }

        byte[] decryptedData;
        try
        {
            var tripleDes = TripleDES.Create();
            tripleDes.Mode = CipherMode.CBC;
            tripleDes.Padding = PaddingMode.None;

            var outKey = new byte[24];
            Buffer.BlockCopy(key, 0, outKey, 0, outKey.Length);

            var transform = tripleDes.CreateDecryptor(outKey, iv);
            decryptedData = transform.TransformFinalBlock(source, offset, length);
        }
        catch (Exception ex)
        {
            throw new SnmpException(
                "Exception was thrown while TripleDES privacy protocol was decrypting data.", ex);
        }

        return decryptedData;
    }

    internal static byte[] DecryptByAes128(this byte[] source, int offset, int length, byte[] key, int engineBoots, int engineTime, byte[] privacyParameters)
    {
        return source.DecryptByAes(offset, length, key, engineBoots, engineTime, 16, privacyParameters);
    }

    internal static byte[] DecryptByAes192(this byte[] source, int offset, int length, byte[] key, int engineBoots, int engineTime, byte[] privacyParameters)
    {
        return source.DecryptByAes(offset, length, key, engineBoots, engineTime, 24, privacyParameters);
    }

    internal static byte[] DecryptByAes256(this byte[] source, int offset, int length, byte[] key, int engineBoots, int engineTime, byte[] privacyParameters)
    {
        return source.DecryptByAes(offset, length, key, engineBoots, engineTime, 32, privacyParameters);
    }

    private static byte[] DecryptByAes(this byte[] source, int offset, int length, byte[] key, int engineBoots, int engineTime, int keyBytes, byte[] privacyParameters)
    {
        byte[] decryptedData;

        var iv = new byte[16];
        var bootsBytes = BitConverter.GetBytes(engineBoots);
        var timeBytes = BitConverter.GetBytes(engineTime);
        iv[0] = bootsBytes[3];
        iv[1] = bootsBytes[2];
        iv[2] = bootsBytes[1];
        iv[3] = bootsBytes[0];
        iv[4] = timeBytes[3];
        iv[5] = timeBytes[2];
        iv[6] = timeBytes[1];
        iv[7] = timeBytes[0];

        Buffer.BlockCopy(privacyParameters, 0, iv, 8, 8);

        var aes = Aes.Create();
        aes.KeySize = keyBytes * 8;
        aes.FeedbackSize = 128;
        aes.BlockSize = 128;
        aes.Padding = PaddingMode.Zeros;
        aes.Mode = CipherMode.CFB;

        if (key.Length > keyBytes)
        {
            var outKey = new byte[keyBytes];
            Buffer.BlockCopy(key, 0, outKey, 0, keyBytes);
            aes.Key = outKey;
        }
        else
        {
            aes.Key = key;
        }

        aes.IV = iv;
        var cryptoTransform = aes.CreateDecryptor();

        var data = new byte[length];
        Buffer.BlockCopy(source, offset, data, 0, length);

        if (data.Length % keyBytes != 0)
        {
            var buffer = new byte[length];
            Buffer.BlockCopy(data, 0, buffer, 0, length);
            var div = (int)Math.Floor(buffer.Length / (double)16);

            var newLength = (div + 1) * 16;
            var decryptBuffer = new byte[newLength];
            Buffer.BlockCopy(buffer, 0, decryptBuffer, 0, buffer.Length);
            decryptedData = cryptoTransform.TransformFinalBlock(decryptBuffer, 0, decryptBuffer.Length);
            Buffer.BlockCopy(decryptedData, 0, buffer, 0, length);

            return buffer;
        }

        decryptedData = cryptoTransform.TransformFinalBlock(data, 0, length);

        return decryptedData;
    }
}