using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SnmpConverter;

/// <summary>
/// SNMP object identifier.
/// </summa
public class SnmpOid : IEnumerable<uint>, IEquatable<SnmpOid>
{
    /// <summary>
    /// Object identifier.
    /// </summary>
    private uint[] _values;

    /// <summary>
    /// Initializes a new instance of the <see cref="SnmpOid"/> class.
    /// </summary>
    public SnmpOid()
    {
        _values = Array.Empty<uint>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SnmpOid"/> class with default value.
    /// </summary>
    /// <param name="value">Default value.</param>
    public SnmpOid(uint value)
    {
        _values = new []{ value };
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SnmpOid"/> class with default values.
    /// </summary>
    /// <param name="value">Default values.</param>
    public SnmpOid(IEnumerable<uint>? value)
    {
        _values = value?.ToArray() ?? Array.Empty<uint>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SnmpOid"/> class with <see cref="string"/> default values.
    /// </summary>
    /// <param name="value"><see cref="string"/> default values.</param>
    /// <exception cref="SnmpException"></exception>
    public SnmpOid(string? value)
    {
        try
        {
            _values = string.IsNullOrEmpty(value)
                ? Array.Empty<uint>()
                : value
                    .Split(".", StringSplitOptions.RemoveEmptyEntries)
                    .Cast<uint>()
                    .ToArray();
        }
        catch (Exception ex)
        {
            throw new SnmpException($"Cannot convert \"{value}\" to oid", ex);
        }
    }

    /// <summary>
    /// Add value to <see cref="SnmpOid">object identifier</see>.
    /// </summary>
    /// <param name="value">Value</param>
    public void Add(uint value)
    {
        AddRange(new[] {value});
    }

    /// <summary>
    /// Add values to <see cref="SnmpOid">object identifier</see>.
    /// </summary>
    /// <param name="value">Values</param>
    public void AddRange(IEnumerable<uint>? value)
    {
        if (value != null && value.Any())
        {
            _values = _values.Concat(value).ToArray();
        }
    }

    /// <summary>
    /// Create an array from <see cref="SnmpOid">object identifier</see>.
    /// </summary>
    /// <returns></returns>
    public uint[] ToArray()
    {
        return _values;
    }

    /// <summary>
    /// Create a <see cref="string"/> from <see cref="SnmpOid">object identifier</see>.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return string.Join('.', _values);
    }

    /// <summary>
    /// Get <see cref="IEnumerable{uint}"/>
    /// </summary>
    /// <returns></returns>
    public IEnumerator<uint> GetEnumerator()
    {
        return (IEnumerator<uint>)_values.GetEnumerator();
    }

    /// <summary>
    /// Get <see cref="IEnumerator"/>
    /// </summary>
    /// <returns></returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Comapre two <see cref="SnmpOid">object identifiers</see>.
    /// </summary>
    /// <param name="other">Another <see cref="SnmpOid">object identifier</see>.</param>
    /// <returns></returns>
    public bool Equals(SnmpOid? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        var otherValues = other.ToArray();
        if (_values.Length != otherValues.Length) return false;
            
        return !_values.Where((item, index) => item != otherValues[index]).Any();
    }

    /// <summary>
    /// Compare <see cref="SnmpOid">object identifier</see> with another object.
    /// </summary>
    /// <param name="obj">Another object.</param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;

        return obj.GetType() == GetType() && Equals((SnmpOid) obj);
    }

    /// <summary>
    /// Get hash code of <see cref="SnmpOid">object identifier</see>.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return _values.GetHashCode();
    }
}