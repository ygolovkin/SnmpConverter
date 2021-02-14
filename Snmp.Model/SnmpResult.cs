namespace Snmp.Model
{
    public class SnmpResult<T>
    {
        public T? Value { get; private set; }

        public string? Error { get; set; }

        public bool HasValue => Error == null;

        public SnmpResult(T value)
        {
            Value = value;
        }

        public SnmpResult(string error)
        {
            Error = error;
        }

        public static implicit operator bool(SnmpResult<T> result)
        {
            return result.HasValue;
        }
    }
}
