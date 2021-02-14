using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Snmp.Model.Packet
{
    public class SnmpEngineId : IEnumerable
    {
        private readonly byte[] _engineId;

        public int Count => _engineId.Length;

        public byte this[int i]
        {
            get => _engineId[i];
            set => _engineId[i] = value;
        }

        public SnmpEngineId()
        {
            _engineId = Array.Empty<byte>();
        }

        public SnmpEngineId(IEnumerable<byte> enigineId)
        {
            _engineId = enigineId is null ? Array.Empty<byte>() : enigineId.ToArray();
        }

        public SnmpEngineId(string engineId)
        {
            _engineId = engineId is null || engineId.Length == 0 || engineId.Length % 2 != 0
                ? Array.Empty<byte>()
                : Enumerable.Range(0, engineId.Length)
                     .Where(x => x % 2 == 0)
                     .Select(x => Convert.ToByte(engineId.Substring(x, 2), 16))
                     .ToArray();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public SnmpEngineIdEnumerator GetEnumerator()
        {
            return new SnmpEngineIdEnumerator(_engineId);
        }

        public static bool operator !=(SnmpEngineId x, SnmpEngineId y)
        {
            return !x.Equals(y);
        }

        public static bool operator ==(SnmpEngineId x, SnmpEngineId y)
        {
            return x.Equals(y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is not SnmpEngineId engineId || _engineId.Length != engineId.Count)
            {
                return false;
            }

            for (int i = 0; i < _engineId.Length; i++)
            {
                if (_engineId[i] != engineId[i]) return false;
            }
            return true;
        }

        public byte[] ToArray()
        {
            return _engineId;
        }

        public override string ToString()
        {
            return BitConverter.ToString(_engineId).Replace("-", "");
        }
    }


    public class SnmpEngineIdEnumerator : IEnumerator
    {
        private readonly byte[] _engineId;
        private int position = -1;

        public byte Current => _engineId[position];

        object IEnumerator.Current => Current;

        public SnmpEngineIdEnumerator(byte[] engineId)
        {
            _engineId = engineId;
        }

        public void Dispose()
        {
            
        }

        public bool MoveNext()
        {
            position++;
            return (position < _engineId.Length);
        }

        public void Reset()
        {
            position = -1;
        }
    }
}
