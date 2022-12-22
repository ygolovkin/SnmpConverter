using System;

namespace SnmpConverter;

internal class SnmpHandlerError<T>
{
    internal Func<T, bool> Predicate { get; private set; }

    internal string Message { get; private set; }

    public SnmpHandlerError(Func<T, bool> predicate, string message)
    {
        Predicate = predicate;
        Message = message;
    }
}