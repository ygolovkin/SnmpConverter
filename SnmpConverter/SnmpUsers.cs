﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SnmpConverter;

public static class SnmpUsers
{
    private static readonly List<SnmpUser> _users = new();

    public static void Add(SnmpUser user)
    {
        if (user is null)
        {
            throw new SnmpException("Users can't be null.", new ArgumentNullException(nameof(user)));
        }

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
            CheckUser(user);
        }

        _users.AddRange(users);
    }

    internal static SnmpResult<bool> IsUserExist(this SnmpUser user)
    {
        var snmpUser = _users.FirstOrDefault(u => u.Username == user.Username);
        if (snmpUser is null)
        {
            return new SnmpResult<bool>($"User {user.Username} doesn't exist.");
        }

        return new SnmpResult<bool>(true);
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