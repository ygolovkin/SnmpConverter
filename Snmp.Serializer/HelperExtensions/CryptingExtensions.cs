using Snmp.Model;
using Snmp.Model.Enums;
using Snmp.Model.Packet;
using Snmp.Model.Users;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Snmp.Serializer.HelperExtensions
{
    public static class CryptingExtensions
    {
        public static SnmpResult<byte[]> GetHash(this SnmpUserAuthentication userAuthentication, byte[] buffer, SnmpEngineId engineId)
        {
            var key = userAuthentication.PasswordToKey(engineId);
            if (key)
            {
                return userAuthentication.Authentication switch
                {
                    SnmpAuthentication.MD5 => new HMACMD5(key.Value).GetHash(buffer),
                    SnmpAuthentication.SHA1 => new HMACSHA1(key.Value).GetHash(buffer),
                    _ => new SnmpResult<byte[]>(Array.Empty<byte>())
                };
            }
            return new SnmpResult<byte[]>(key.Error);
        }

        public static SnmpResult<byte[]> PasswordToKey(this SnmpUserAuthentication userAuthentication, SnmpEngineId engineId)
        {
            if(userAuthentication.Authentication == SnmpAuthentication.None)
            {
                return new SnmpResult<byte[]>(Array.Empty<byte>());
            }

            var key = Encoding.UTF8.GetBytes(userAuthentication.Key);

            var algoritmh = userAuthentication.Authentication == SnmpAuthentication.MD5 
                ? new MD5CryptoServiceProvider()
                : (HashAlgorithm)new SHA1CryptoServiceProvider();

            byte[] tempary = new byte[1048576];

            int count = 1048576 / key.Length;
            for (int i = 0; i < count; i++)
            {
                Buffer.BlockCopy(key, 0, tempary, i * key.Length, key.Length);
            }
            
            int remainder = 1048576 - (key.Length * count);
            if (remainder != 0) Buffer.BlockCopy(key, 0, tempary, key.Length * count, remainder);

            byte[] hash = algoritmh.ComputeHash(tempary);
            byte[] tmp = new byte[hash.Length + hash.Length + engineId.Length];
            Buffer.BlockCopy(hash, 0, tmp, 0, hash.Length);
            Buffer.BlockCopy(engineId.ToArray(), 0, tmp, hash.Length, engineId.Length);
            Buffer.BlockCopy(hash, 0, tmp, hash.Length + engineId.Length, hash.Length);

            byte[] result = algoritmh.ComputeHash(tmp);
            algoritmh.Clear();

            return new SnmpResult<byte[]>(result);
        }

        private static SnmpResult<byte[]> GetHash(this HMAC hmac, byte[] buffer)
        {
            var hash = hmac.ComputeHash(buffer);
            var result = new byte[12];
            Buffer.BlockCopy(hash, 0, result, 0, 12);
            hmac.Clear();
            return new SnmpResult<byte[]>(result);
        }
    }
}
