﻿namespace Snmp.Model
{
    public class SnmpResult<T>
    {
        private readonly string? _error;

        public T Value { get; private set; }

        public string Error => _error is null ? string.Empty : _error;

        public bool HasValue => _error is null;

        public SnmpResult(T value)
        {
            Value = value;
        }

        public SnmpResult(string error)
        {
            _error = error;
        }

        public static implicit operator bool(SnmpResult<T> result)
        {
            return result.HasValue;
        }
    }
}
