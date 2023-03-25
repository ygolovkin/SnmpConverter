using System;
using System.Collections.Generic;
using System.Linq;

namespace SnmpConverter;

public static class SnmpUsers
{
    private static List<SnmpUser> _users = new()
    {
        new SnmpUser
        {
            Username = string.Empty,
            AuthenticationType = SnmpAuthenticationType.None,
            PrivacyType = SnmpPrivacyType.None,
            Password = string.Empty,
            Key = string.Empty,
            HashPassword = Array.Empty<byte>(),
            HashKey = Array.Empty<byte>(),
            EngineId = new SnmpEngineId(),
            ContextEngineId = new SnmpEngineId(),
            ContextName = string.Empty
        }
    };

    public static void Add(SnmpUser user)
    {
        if (user is null)
        {
            throw new SnmpException("Users can't be null.", new ArgumentNullException(nameof(user)));
        }

        CheckUser(user);

        AddRange(new[] { user });
    }

    public static void AddRange(IEnumerable<SnmpUser> users)
    {
        if (users is null)
        {
            throw new SnmpException("Users can't be null.", new ArgumentNullException(nameof(users)));
        }

        foreach (var user in users)
        {
            var existUser = _users.FirstOrDefault(u => u.Username == user.Username);
            if(existUser is not null)
            {
                throw new SnmpException($"User with username: {user.Username} already exists.", new ArgumentNullException(nameof(user.Username)));
            }
            CheckUser(user);
        }

        _users.AddRange(users);
    }

    public static void Delete(string username)
    {
        if(username is not null)
        {
            _users = _users.Where(u => u.Username != username).ToList();
        }
    }

    public static void Clear()
    {
        _users.Clear();
    }

    internal static SnmpResult<SnmpUser> Get(string username)
    {
        var snmpUser = _users.FirstOrDefault(u => u.Username == username);
        if (snmpUser is null)
        {
            return new SnmpResult<SnmpUser>($"User {username} doesn't exist.");
        }

        return new SnmpResult<SnmpUser>(snmpUser);
    }

    private static void CheckUser(SnmpUser user)
    {
        if (string.IsNullOrEmpty(user.Username))
        {
            throw new SnmpException("User's name can't be null or empty.",
                new ArgumentException("Incorrect username", nameof(user.Username)));
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

        user.HashPassword();
        user.HashKey();
    }
}