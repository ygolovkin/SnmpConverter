using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SnmpConverter
{
    public class SnmpOid : IEnumerable<uint>, IEquatable<SnmpOid>
    {
        private uint[] _values;
        
        public SnmpOid()
        {
            _values = Array.Empty<uint>();
        }

        public SnmpOid(uint value)
        {
            _values = new []{ value };
        }

        public SnmpOid(IEnumerable<uint>? value)
        {
            _values = value?.ToArray() ?? Array.Empty<uint>();
        }

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

        public void Add(uint value)
        {
            AddRange(new[] {value});
        }

        public void AddRange(IEnumerable<uint>? value)
        {
            if (value != null && value.Any())
            {
                _values = _values.Concat(value).ToArray();
            }
        }

        public uint[] ToArray()
        {
            return _values;
        }

        public override string ToString()
        {
            return string.Join('.', _values);
        }

        public IEnumerator<uint> GetEnumerator()
        {
            return (IEnumerator<uint>)_values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Equals(SnmpOid? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            var otherValues = other.ToArray();
            if (_values.Length != otherValues.Length) return false;
            
            return !_values.Where((item, index) => item != otherValues[index]).Any();
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj.GetType() == GetType() && Equals((SnmpOid) obj);
        }

        public override int GetHashCode()
        {
            return _values.GetHashCode();
        }
    }
}
