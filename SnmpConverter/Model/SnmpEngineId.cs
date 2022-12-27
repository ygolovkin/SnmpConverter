using System;
using System.Collections.Generic;
using System.Linq;

namespace SnmpConverter;

public class SnmpEngineId
{
    private readonly byte[] _engine;

    public SnmpEngineId()
    {
        _engine = Array.Empty<byte>();
    }

    public SnmpEngineId(IEnumerable<byte> engine)
    {
        if (engine is null)
        {
            throw new SnmpException("EngineId can't be null.", new ArgumentNullException(nameof(engine)));
        }

        var tmp = engine.ToArray();

        if (tmp.Length is < 10 or > 24)
        {
            throw new SnmpException("EngineId must no less than 5 bytes and no more than 12 bytes.",
                new ArgumentException("Incorrect engineId", nameof(engine)));
        }

        _engine = tmp;
    }

    public SnmpEngineId(string engine)
    {
        if (engine is null)
        {
            throw new SnmpException("EngineId can't be null.", new ArgumentNullException(nameof(engine)));
        }

        if (engine.Length % 2 != 0 || engine.Length is < 10 or > 24)
        {
            throw new SnmpException("EngineId must be even, no less than 10 and no more than 24.",
                new ArgumentException("Incorrect engineId", nameof(engine)));
        }

        _engine = Enumerable.Range(0, engine.Length)
            .Where(x => x % 2 == 0)
            .Select(x => Convert.ToByte(engine.Substring(x, 2), 16))
            .ToArray();
    }

    public byte[] ToArray()
    {
        return _engine;
    }

    public override string ToString()
    {
        return BitConverter.ToString(_engine).Replace("-", "");
    }
}