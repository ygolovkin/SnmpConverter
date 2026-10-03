using SnmpConverter.Models.Enums;
using System.Security.Cryptography;
using System.Text;

namespace SnmpConverter.Extensions;

internal static class HashExtensions
{
    internal static void HashPassword(this SnmpUser user)
    {
        if (user.AuthenticationType == SnmpAuthenticationType.None)
        {
            user.HashPassword = [];
        }

        user.HashPassword = Encoding.UTF8
            .GetBytes(user.Password)
            .HashPassword(user.EngineId.ToArray(), user.AuthenticationType);
    }

    internal static void HashKey(this SnmpUser user)
    {
        if (user.PrivacyType == SnmpPrivacyType.None)
        {
            user.HashKey = [];
        }

        byte[] key = Encoding.UTF8.GetBytes(user.Key);
        byte[] hashKey = key.HashPassword(user.EngineId.ToArray(), user.AuthenticationType);
        byte[] engineId = user.EngineId.ToArray();

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
        if (user is null)
        {
            return buffer;
        }

        return buffer.GetHash(user.AuthenticationType, user.HashPassword);
    }

    internal static byte[] GetHash(this byte[] buffer, SnmpAuthenticationType authenticationType, byte[] key = null)
    {
        if (authenticationType == SnmpAuthenticationType.None)
        {
            return buffer;
        }

        HMAC hmac = authenticationType switch
        {
            SnmpAuthenticationType.MD5 => key is null ? new HMACMD5() : new HMACMD5(key),
            SnmpAuthenticationType.SHA1 => key is null ? new HMACSHA1() : new HMACSHA1(key),
            SnmpAuthenticationType.SHA256 => key is null ? new HMACSHA256() : new HMACSHA256(key),
            SnmpAuthenticationType.SHA384 => key is null ? new HMACSHA384() : new HMACSHA384(key),
        };

        var hash = hmac.ComputeHash(buffer, 0, buffer.Length);
        var result = new byte[12];
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

        HashAlgorithm hashAlgorithm = authenticationType switch
        {
            SnmpAuthenticationType.MD5 => MD5.Create(),
            SnmpAuthenticationType.SHA1 => SHA1.Create(),
            SnmpAuthenticationType.SHA256 => SHA256.Create(),
            SnmpAuthenticationType.SHA384 => SHA384.Create(),
        };

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
        int length = hasKey.Length;

        int minBytesLength = authenticationType switch
        {
            SnmpAuthenticationType.MD5 => 16,
            SnmpAuthenticationType.SHA1 => 20,
            SnmpAuthenticationType.SHA256 => 32,
            SnmpAuthenticationType.SHA384 => 48,
        };

        byte[] extendedKey = new byte[32];
        Buffer.BlockCopy(hasKey, 0, extendedKey, 0, hasKey.Length);

        while (length < 32)
        {
            byte[] key = hasKey.HashPassword(engineId, authenticationType);
            int copyBytes = Math.Min(32 - length, minBytesLength);

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