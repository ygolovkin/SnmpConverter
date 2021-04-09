using Snmp.Model.Exceptions;

namespace Snmp.Model
{
    public class SnmpResult<T>
    {
        private readonly string? _error;

        public T Value { get; } = default!;

        public string Error => _error ?? string.Empty;

        public bool HasValue => _error is null;

        public bool HasError => !HasValue;

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

        public void HandleError()
        {
            if(HasError) throw new SnmpException(Error);
        }
    }
}
