using System;
using System.Security.Cryptography;

namespace SnmpConverter;

internal static class EncryptingExtensions
{
    internal static SnmpResult<byte[]> DecryptByDes(this byte[]? source, int offset, int length, byte[] key, byte[] privacyParameters)
    {
        if (length % 8 != 0
            || source == null
            || source.Length == 0
            || offset > source.Length
            || offset + length > source.Length)
        {
            return new SnmpResult<byte[]>("Incorrect packet data.");
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

        return new SnmpResult<byte[]>(decryptedData);
    }

    internal static SnmpResult<byte[]> DecryptByTripleDes(this byte[]? source, int offset, int length, byte[] key, byte[] privacyParameters)
    {
        if (length % 8 != 0
            || source == null
            || source.Length == 0
            || offset > source.Length
            || offset + length > source.Length)
        {
            return new SnmpResult<byte[]>("Incorrect packet data.");
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

        return new SnmpResult<byte[]>(decryptedData);
    }

    internal static SnmpResult<byte[]> DecryptByAes128(this byte[] source, int offset, int length, byte[] key, int engineBoots, int engineTime, byte[] privacyParameters)
    {
        return source.DecryptByAes(offset, length, key, engineBoots, engineTime, privacyParameters, 16);
    }

    internal static SnmpResult<byte[]> DecryptByAes192(this byte[] source, int offset, int length, byte[] key, int engineBoots, int engineTime, byte[] privacyParameters)
    {
        return source.DecryptByAes(offset, length, key, engineBoots, engineTime, privacyParameters, 24);
    }

    internal static SnmpResult<byte[]> DecryptByAes256(this byte[] source, int offset, int length, byte[] key, int engineBoots, int engineTime, byte[] privacyParameters)
    {
        return source.DecryptByAes(offset, length, key, engineBoots, engineTime, privacyParameters, 32);
    }

    private static SnmpResult<byte[]> DecryptByAes(this byte[] source, int offset, int length, byte[] Key, int engineBoots, int engineTime, byte[] privacyParameters, int keyBytes)
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

        var rm = Aes.Create();
        rm.KeySize = keyBytes * 8;
        rm.FeedbackSize = 128;
        rm.BlockSize = 128;
        rm.Padding = PaddingMode.Zeros;
        rm.Mode = CipherMode.CFB;

        if (Key.Length > keyBytes)
        {
            var outKey = new byte[keyBytes];
            Buffer.BlockCopy(Key, 0, outKey, 0, keyBytes);
            rm.Key = outKey;
        }
        else
        {
            rm.Key = Key;
        }

        rm.IV = iv;
        var cryptoTransform = rm.CreateDecryptor();

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

            return new SnmpResult<byte[]>(buffer);
        }

        decryptedData = cryptoTransform.TransformFinalBlock(data, 0, length);

        return new SnmpResult<byte[]>(decryptedData);
    }
}