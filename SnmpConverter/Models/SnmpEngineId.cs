using System;
using System.Collections.Generic;
using System.Linq;

namespace SnmpConverter;

/// <summary>
/// SNMP engine identifier.
/// </summary>
public class SnmpEngineId : IEquatable<SnmpEngineId>
{
    /// <summary>
    /// Engine identifier.
    /// </summary>
    private readonly byte[] _engineId;

    /// <summary>
    /// Get total numbers of elements.
    /// </summary>
    public int Length => _engineId.Length;

    /// <summary>
    /// Is EngineId's length zero.
    /// </summary>
    public bool IsEmpty => Length == 0;

    /// <summary>
    /// Initializes a new instance of the <see cref="SnmpEngineId"/> class.
    /// </summary>
    public SnmpEngineId()
    {
        _engineId = Array.Empty<byte>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SnmpEngineId"/> class with collection of bytes.
    /// </summary>
    /// <param name="engineId">EngineId's collection of bytes.</param>
    /// <exception cref="SnmpException"></exception>
    public SnmpEngineId(IEnumerable<byte> engineId)
    {
        if (engineId is null)
        {
            throw new SnmpException("EngineId can't be null.", new ArgumentNullException(nameof(engineId)));
        }

        var buffer = engineId.ToArray();

        if (buffer.Length != 0 && buffer.Length is < 10 or > 24)
        {
            throw new SnmpException("EngineId must no less than 5 bytes and no more than 12 bytes.",
                new ArgumentException("Incorrect engineId", nameof(engineId)));
        }

        _engineId = buffer;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SnmpEngineId"/> class with string.
    /// </summary>
    /// <param name="engineId">EngineId's string.</param>
    /// <exception cref="SnmpException"></exception>
    public SnmpEngineId(string engineId)
    {
        if (engineId is null)
        {
            throw new SnmpException("EngineId can't be null.", new ArgumentNullException(nameof(engineId)));
        }

        if (engineId.Length != 0 && (engineId.Length % 2 != 0 || engineId.Length is < 10 or > 24))
        {
            throw new SnmpException("EngineId must be even, no less than 10 and no more than 24.",
                new ArgumentException("Incorrect engineId", nameof(engineId)));
        }

        _engineId = Enumerable.Range(0, engineId.Length)
            .Where(x => x % 2 == 0)
            .Select(x => Convert.ToByte(engineId.Substring(x, 2), 16))
            .ToArray();
    }

    /// <summary>
    /// Create an array from <see cref="SnmpEngineId">engine identifier</see>.
    /// </summary>
    /// <returns></returns>
    public byte[] ToArray()
    {
        return _engineId;
    }

    /// <summary>
    /// Create a <see cref="string"/> from <see cref="SnmpEngineId">engine identifier</see>.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return BitConverter.ToString(_engineId).Replace("-", "");
    }

    /// <summary>
    /// Compare two <see cref="SnmpEngineId">engine identifiers</see>.
    /// </summary>
    /// <param name="other">Another <see cref="SnmpEngineId">engine identifier</see>.</param>
    /// <returns></returns>
    public bool Equals(SnmpEngineId? other)
    {
        if(other is null)
        {
            return false;
        }

        return other.ToArray().IsEqual(_engineId);
    }
}