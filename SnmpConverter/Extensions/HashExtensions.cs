using System;
using System.Security.Cryptography;
using System.Text;

namespace SnmpConverter;

internal static class HashExtensions
{
    internal static void HashPassword(this SnmpUser user)
    {
        if (user.AuthenticationType == SnmpAuthenticationType.None)
        {
            user.HashPassword = Array.Empty<byte>();
        }

        user.HashPassword = Encoding.UTF8.GetBytes(user.Password!)
            .HashPassword(user.EngineId!.ToArray(), user.AuthenticationType);
    }

    internal static void HashKey(this SnmpUser user)
    {
        if (user.PrivacyType == SnmpPrivacyType.None)
        {
            user.HashKey = Array.Empty<byte>();
        }
        
        var key = Encoding.UTF8.GetBytes(user.Key);
        var hashKey = key.HashPassword(user.EngineId!.ToArray(), user.AuthenticationType);
        var engineId = user.EngineId!.ToArray();

        user.HashKey = user.PrivacyType switch
        {
            SnmpPrivacyType.TripleDes => hashKey.ExtendShortKey3DES(engineId, user.AuthenticationType),
            SnmpPrivacyType.Aes192 => hashKey.ExtendShortKeyAES(engineId, user.AuthenticationType, user.PrivacyType),
            SnmpPrivacyType.Aes256 => hashKey.ExtendShortKeyAES(engineId, user.AuthenticationType, user.PrivacyType),
            _ => hashKey
        };
    }

    internal static byte[] GetHash(this byte[] buffer, SnmpUser? user)
    {
        if (user is null || user.AuthenticationType == SnmpAuthenticationType.None)
        {
            return buffer;
        }

        HMAC hmac = user.AuthenticationType == SnmpAuthenticationType.MD5
            ? new HMACMD5(user.HashPassword)
            : new HMACSHA1(user.HashPassword);
        var result = new byte[12];
        
        var hash = hmac.ComputeHash(buffer);
        Buffer.BlockCopy(hash, 0, result, 0, 12);
        hmac.Clear();

        return result;
    }

    private static byte[] HashPassword(this byte[] password, byte[] engineId, SnmpAuthenticationType authenticationType)
    {
        var bytes = new byte[1048576];
        var count = 1048576 / password.Length;
        for (var i = 0; i < count; i++)
        {
            Buffer.BlockCopy(password, 0, bytes, i * password.Length, password.Length);
        }
        var remainder = 1048576 - password.Length * count;
        if (remainder != 0)
        {
            Buffer.BlockCopy(password, 0, bytes, password.Length * count, remainder);
        }

        HashAlgorithm hashAlgorithm = authenticationType == SnmpAuthenticationType.MD5
            ? MD5.Create()
            : SHA1.Create();
        var hash = hashAlgorithm.ComputeHash(bytes);

        var buffer = new byte[hash.Length + hash.Length + engineId.Length];
        Buffer.BlockCopy(hash, 0, buffer, 0, hash.Length);
        Buffer.BlockCopy(engineId, 0, buffer, hash.Length, engineId.Length);
        Buffer.BlockCopy(hash, 0, buffer, hash.Length + engineId.Length, hash.Length);

        var computeHash = hashAlgorithm.ComputeHash(buffer);
        hashAlgorithm.Clear();

        return computeHash;
    }

    private static byte[] ExtendShortKey3DES(this byte[] hasKey, byte[] engineId, SnmpAuthenticationType authenticationType)
    {
        var length = hasKey.Length;

        var minBytesLength = authenticationType == SnmpAuthenticationType.MD5 ? 16 : 20;

        var extendedKey = new byte[32];
        Buffer.BlockCopy(hasKey, 0, extendedKey, 0, hasKey.Length);

        while (length < 32)
        {
            var key = hasKey.HashPassword(engineId, authenticationType);
            var copyBytes = Math.Min(32 - length, minBytesLength);

            Buffer.BlockCopy(key, 0, extendedKey, length, copyBytes);
            length += copyBytes;
        }
        return extendedKey;
    }

    private static byte[] ExtendShortKeyAES(this byte[] shortKey, byte[] engineId, SnmpAuthenticationType authenticationType, SnmpPrivacyType privacyType)
    {
        var minKeyLength = privacyType == SnmpPrivacyType.Aes256 ? 32 : 24;

        var extendShortKey = new byte[minKeyLength];
        var keyBuffer = new byte[shortKey.Length];
        Array.Copy(shortKey, keyBuffer, shortKey.Length);

        var keyLength = shortKey.Length > minKeyLength ? minKeyLength : shortKey.Length;
        Array.Copy(shortKey, extendShortKey, keyLength);

        while (keyLength < minKeyLength)
        {
            var bytes = keyBuffer.HashPassword(engineId, authenticationType);

            if (bytes.Length <= minKeyLength - keyLength)
            {
                Array.Copy(bytes, 0, extendShortKey, keyLength, bytes.Length);
                keyLength += bytes.Length;
            }
            else
            {
                Array.Copy(bytes, 0, extendShortKey, keyLength, minKeyLength - keyLength);
                keyLength += minKeyLength - keyLength;
            }

            keyBuffer = new byte[bytes.Length];
            Array.Copy(bytes, keyBuffer, bytes.Length);
        }
        return extendShortKey;
    }
}