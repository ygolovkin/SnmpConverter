using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Snmp.Model.Exceptions;

namespace Snmp.Model.Packet
{
    public class Oid : IEnumerable<uint>, IEquatable<Oid>
    {
        private uint[] values;
        
        public Oid()
        {
            values = Array.Empty<uint>();
        }

        public Oid(uint value)
        {
            values = new []{ value };
        }

        public Oid(IEnumerable<uint> value)
        {
            values = value?.ToArray() ?? Array.Empty<uint>();
        }

        public Oid(string value)
        {
            try
            {
                values = string.IsNullOrEmpty(value)
                ? Array.Empty<uint>()
                : value
                    .Split(".", StringSplitOptions.RemoveEmptyEntries)
                    .Select(str => Convert.ToUInt32(str))
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

        public void AddRange(IEnumerable<uint> value)
        {
            if(value != null && value.Any()) 
                values = values.Concat(value).ToArray();
        }

        public uint[] ToArray()
        {
            return values;
        }

        public override string ToString()
        {
            return string.Join('.', values);
        }

        public IEnumerator<uint> GetEnumerator()
        {
            return (IEnumerator<uint>)values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Equals(Oid? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            var otherValues = other.ToArray();
            if (values.Length != otherValues.Length) return false;
            
            return !values.Where((t, i) => t != otherValues[i]).Any();
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj.GetType() == GetType() && Equals((Oid) obj);
        }

        public override int GetHashCode()
        {
            return values.GetHashCode();
        }
    }
}
