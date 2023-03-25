using System;
using System.Security.Cryptography;

namespace SnmpConverter;
internal static class EncryptingExtensions
{
    private static bool isSaltIncrement = true;
    private static int salt = -1;

    internal static SnmpResult<byte[]> EncryptByDes(this byte[] source, byte[] key, int engineBoots, out byte[] privacyParameters)
    {
        privacyParameters = new byte[8];
        if (key == null || key.Length < 16)
        {
            return new SnmpResult<byte[]>("Incorrect data.");
        }

        InitSalt();
        privacyParameters = CreateKey(engineBoots, salt, 8);

        byte[] iv = new byte[8];
        for (int i = 0; i < iv.Length; i++)
        {
            iv[i] = (byte)(privacyParameters[i] ^ key[8 + i]);
        }            
        byte[] outKey = new byte[8];
        Buffer.BlockCopy(key, 0, outKey, 0, 8);

        int div = (int)Math.Floor(source.Length / 8.0);
        if ((source.Length % 8) != 0)
        {
            div += 1;
        }
        int newLength = div * 8;
        byte[] result = new byte[newLength];
        byte[] buffer = new byte[newLength];

        byte[] inbuffer = new byte[8];
        byte[] cipherText = iv;
        int posIn = 0;
        int posResult = 0;
        Buffer.BlockCopy(source, 0, buffer, 0, source.Length);

        var des = DES.Create();
        des.Mode = CipherMode.ECB;
        des.Padding = PaddingMode.None;

        ICryptoTransform transform = des.CreateEncryptor(outKey, null);
        for (int i = 0; i < div; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                inbuffer[j] = (byte)(buffer[posIn] ^ cipherText[j]);
                posIn++;
            }
            transform.TransformBlock(inbuffer, 0, inbuffer.Length, cipherText, 0);
            Buffer.BlockCopy(cipherText, 0, result, posResult, cipherText.Length);
            posResult += cipherText.Length;
        }

        des.Clear();
        return new SnmpResult<byte[]>(result);
    }

    internal static SnmpResult<byte[]> EncryptBy3Des(this byte[] source, byte[] key, int engineBoots, SnmpAuthenticationType authenticationType, out byte[] privacyParameters)
    {
        InitSalt();
        privacyParameters = CreateKey(engineBoots, salt, 8);

        var privacyParametersHash = privacyParameters.GetHash(authenticationType);
        privacyParameters = new byte[8];
        Buffer.BlockCopy(privacyParametersHash, 0, privacyParameters, 0, 8);
        byte[] iv = new byte[8];
        for (int i = 0; i < iv.Length; i++)
        {
            iv[i] = (byte)(privacyParameters[i] ^ key[24 + i]);
        }

        byte[] encryptedData;
        try
        {
            var tripleDes = TripleDES.Create();
            tripleDes.Mode = CipherMode.CBC;
            tripleDes.Padding = PaddingMode.None;
            byte[] normKey = new byte[24];
            Buffer.BlockCopy(key, 0, normKey, 0, normKey.Length);

            ICryptoTransform transform = tripleDes.CreateEncryptor(normKey, iv);
            if ((source.Length % 8) == 0)
            {
                encryptedData = transform.TransformFinalBlock(source, 0, source.Length);
            }
            else
            {
                byte[] tmpbuffer = new byte[8 * ((source.Length / 8) + 1)];
                Buffer.BlockCopy(source, source.Length, tmpbuffer, 0, source.Length);
                encryptedData = transform.TransformFinalBlock(tmpbuffer, 0, tmpbuffer.Length);
            }
        }
        catch (Exception ex)
        {
            throw new SnmpException("Encrypt error.", ex);
        }

        return new SnmpResult<byte[]>(encryptedData);
    }

    internal static SnmpResult<byte[]> EncryptByAes128(this byte[] source, byte[] key, int engineBoots, int engineTime, out byte[] privacyParameters)
    {        
        return source.EncryptByAes(key, engineBoots, engineTime, 16, out privacyParameters);
    }

    internal static SnmpResult<byte[]> EncryptByAes192(this byte[] source, byte[] key, int engineBoots, int engineTime, out byte[] privacyParameters)
    {        
        return source.EncryptByAes(key, engineBoots, engineTime, 24, out privacyParameters);
    }

    internal static SnmpResult<byte[]> EncryptByAes256(this byte[] source, byte[] key, int engineBoots, int engineTime, out byte[] privacyParameters)
    {        
        return source.EncryptByAes(key, engineBoots, engineTime, 32, out privacyParameters);
    }

    private static SnmpResult<byte[]> EncryptByAes(this byte[] source, byte[] key, int engineBoots, int engineTime, int keyBytes, out byte[] privacyParameters)
    {
        privacyParameters = new byte[8];
        if (key == null || key.Length < keyBytes)
        {
            return new SnmpResult<byte[]>("Incorrect data.");
        }

        var iv = CreateKey(engineBoots, engineTime, 16);

        InitSalt();
        var saltBytes = BitConverter.GetBytes(salt);
        privacyParameters[0] = saltBytes[7];
        privacyParameters[1] = saltBytes[6];
        privacyParameters[2] = saltBytes[5];
        privacyParameters[3] = saltBytes[4];
        privacyParameters[4] = saltBytes[3];
        privacyParameters[5] = saltBytes[2];
        privacyParameters[6] = saltBytes[1];
        privacyParameters[7] = saltBytes[0];
        
        Buffer.BlockCopy(privacyParameters, 0, iv, 8, 8);

        var aes = Aes.Create();
        aes.KeySize = keyBytes * 8;
        aes.FeedbackSize = 128;
        aes.BlockSize = 128;
        aes.Padding = PaddingMode.Zeros;
        aes.Mode = CipherMode.CFB;

        var pkey = new byte[keyBytes];
        Buffer.BlockCopy(key, 0, pkey, 0, keyBytes);
        aes.Key = pkey;
        aes.IV = iv;

        var cryptor = aes.CreateEncryptor();
        var encryptedData = cryptor.TransformFinalBlock(source, 0, source.Length);
        if (encryptedData.Length != source.Length)
        {
            byte[] tmp = new byte[source.Length];
            Buffer.BlockCopy(encryptedData, 0, tmp, 0, source.Length);
            return new SnmpResult<byte[]>(tmp);
        }
        return new SnmpResult<byte[]>(encryptedData);
    }

    private static byte[] CreateKey(int first, int second, int size)
    {
        var key = new byte[size];

        var firstBytes = BitConverter.GetBytes(first);
        key[3] = firstBytes[0];
        key[2] = firstBytes[1];
        key[1] = firstBytes[2];
        key[0] = firstBytes[3];

        var secondBytes = BitConverter.GetBytes(second);
        key[7] = secondBytes[0];
        key[6] = secondBytes[1];
        key[5] = secondBytes[2];
        key[4] = secondBytes[3];

        return key;
    }

    private static void InitSalt()
    {
        if (salt == -1)
        {
            Random rand = new Random();
            salt = Convert.ToInt32(rand.Next(1, Int32.MaxValue));
        }
        else
        {
            if (isSaltIncrement)
            {
                salt += 1;
                if (salt < 0) 
                {
                    salt = 1;
                }
            }
        }
    }
}