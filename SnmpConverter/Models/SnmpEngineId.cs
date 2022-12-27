using System;
using System.Collections.Generic;
using System.Linq;

namespace SnmpConverter;

public class SnmpEngineId
{
    private readonly byte[] _engineId;

    public int Length => _engineId.Length;

    public bool IsEmpty => Length == 0;

    public SnmpEngineId()
    {
        _engineId = Array.Empty<byte>();
    }

    public SnmpEngineId(IEnumerable<byte> engineId)
    {
        if (engineId is null)
        {
            throw new SnmpException("EngineId can't be null.", new ArgumentNullException(nameof(engineId)));
        }

        var buffer = engineId.ToArray();

        if (buffer.Length != 0 && buffer.Length is < 10 or > 24)
        {
            throw new SnmpException("EngineId must no less than 5 bytes and no more than 12 bytes.",
                new ArgumentException("Incorrect engineId", nameof(engineId)));
        }

        _engineId = buffer;
    }

    public SnmpEngineId(string engineId)
    {
        if (engineId is null)
        {
            throw new SnmpException("EngineId can't be null.", new ArgumentNullException(nameof(engineId)));
        }

        if (engineId.Length != 0 && (engineId.Length % 2 != 0 || engineId.Length is < 10 or > 24))
        {
            throw new SnmpException("EngineId must be even, no less than 10 and no more than 24.",
                new ArgumentException("Incorrect engineId", nameof(engineId)));
        }

        _engineId = Enumerable.Range(0, engineId.Length)
            .Where(x => x % 2 == 0)
            .Select(x => Convert.ToByte(engineId.Substring(x, 2), 16))
            .ToArray();
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