using System;
using System.Collections.Generic;
using System.Linq;

namespace SnmpConverter;

internal static class UsersExtensions
{
    internal static List<SnmpUser> EncodeUsers(this IEnumerable<SnmpUser>? users)
    {
        if(users is null || !users.Any())
        {
            throw new SnmpException("Users can't be null or empty.");
        }

        foreach (var user in users)
        {
            user.CheckUser();
        }
        
        return users.ToList();
    }

    internal static SnmpUser HandleUsername(this IList<SnmpUser> users, string username)
    {
        var user = users.FirstOrDefault(u => u.Name == username);
        if (user is null)
        {
            throw new SnmpException($"User {username} doesn't exist.");
        }

        return user;
    }

    internal static void CheckUser(this SnmpUser user)
    {
        if (string.IsNullOrEmpty(user.Name))
        {
            throw new SnmpException("User's name can't be null or empty.",
                new ArgumentException("Incorrect username", nameof(user.Name)));
        }

        if (user.EngineId is null)
        {
            throw new SnmpException("EngineId can't be null.", new ArgumentNullException(nameof(user.EngineId)));
        }

        if (user.AuthenticationType != SnmpAuthenticationType.None &&
            (user.Password is null || user.Password.Length < 8))
        {
            throw new SnmpException("Password must be 8 bytes or more.",
                new ArgumentException("Incorrect password.", nameof(user.Password)));
        }

        if (user.PrivacyType != SnmpPrivacyType.None && user.AuthenticationType == SnmpAuthenticationType.None)
        {
            throw new SnmpException("Privacy type can't be none with authentication type equals none.",
                new ArgumentException("Incorrect privacy type.", nameof(user.PrivacyType)));
        }

        if (user.PrivacyType != SnmpPrivacyType.None &&
            (user.Key is null || user.Key.Length < 8))
        {
            throw new SnmpException("Key must be 8 bytes or more.",
                new ArgumentException("Incorrect key.", nameof(user.Key)));
        }

        if(!user.Encoded)
        {
            user.HashPassword();
            user.HashKey();
        }
        user.Encoded = true;
    }
}