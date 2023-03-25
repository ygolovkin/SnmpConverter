using System;

namespace SnmpConverter;

/// <summary>
/// Represents errors that occur during execution.
/// </summary>
public class SnmpException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SnmpException"/> class.
    /// </summary>
    public SnmpException() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SnmpException"/> class with a specified error.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public SnmpException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SnmpException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>

    public SnmpException(string message, Exception innerException) : base(message, innerException) { }

}