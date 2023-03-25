using System;

namespace SnmpConverter;

/// <summary>
/// SNMP result.
/// </summary>
/// <typeparam name="T">The type of object.</typeparam>
internal class SnmpResult<T>
{
    /// <summary>
    /// Error message.
    /// </summary>
    private readonly string? _error;

    /// <summary>
    /// Value.
    /// </summary>
    internal T Value { get; } = default!;

    /// <summary>
    /// Error message.
    /// </summary>
    internal string Error => _error ?? string.Empty;

    /// <summary>
    /// Result has value.
    /// </summary>
    internal bool HasValue => _error is null;

    /// <summary>
    /// Result has error.
    /// </summary>
    internal bool HasError => _error is not null;

    /// <summary>
    /// Initializes a new instance of the <see cref="SnmpResult"/> class with value.
    /// </summary>
    /// <param name="value">Value.</param>
    internal SnmpResult(T value)
    {
        Value = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SnmpResult"/> class with error.
    /// </summary>
    /// <param name="error">Error message.</param>
    internal SnmpResult(string error)
    {
        _error = error;
    }

    /// <summary>
    /// Handle error.
    /// </summary>
    /// <exception cref="SnmpException"></exception>
    internal void HandleError()
    {
        if (HasError)
        {
            throw new SnmpException(_error!);
        }
    }

    /// <summary>
    /// Handle error with predicate
    /// </summary>
    /// <param name="predicate">Predicate for handle.</param>
    /// <param name="message">Error message for unhandling behavior.</param>
    /// <exception cref="SnmpException"></exception>
    internal void HandleError(Func<T, bool>? predicate, string message)
    {
        HandleError();

        if (predicate is not null && predicate(Value))
        {
            throw new SnmpException(message);
        }
    }
}