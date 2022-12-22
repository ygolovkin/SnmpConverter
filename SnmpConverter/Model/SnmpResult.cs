using System;

namespace SnmpConverter
{
    internal class SnmpResult<T>
    {
        private string? _error;

        internal T Value { get; } = default!;

        internal string Error => _error ?? string.Empty;

        internal bool HasValue => _error is null;

        internal bool HasError => _error is not null;

        internal SnmpResult(T value)
        {
            Value = value;
        }

        internal SnmpResult(string error)
        {
            _error = error;
        }

        internal void HandleError()
        {
            if (HasError)
            {
                throw new SnmpException(_error!);
            }
        }

        internal void HandleError(Func<T, bool>? predicate, string message)
        {
            HandleError();

            if (predicate is not null && predicate(Value))
            {
                throw new SnmpException(message);
            }
        }
    }
}
