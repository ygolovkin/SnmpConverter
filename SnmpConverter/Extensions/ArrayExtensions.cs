using System;
using System.Linq;

namespace SnmpConverter;

internal static class ArrayExtensions
{
    internal static byte[] Prepend(this byte[]? source, byte value)
    {
        source ??= Array.Empty<byte>();

        return new[] { value }.Concat(source).ToArray();
    }

    internal static byte[] Prepend(this byte[]? source, byte[]? values)
    {
        source ??= Array.Empty<byte>();
        values ??= Array.Empty<byte>();

        return values.Concat(source).ToArray();
    }

    internal static byte[] Append(this byte[]? source, byte value)
    {
        source ??= Array.Empty<byte>();

        return source.Concat(new[] { value }).ToArray();
    }

    internal static byte[] Append(this byte[]? source, byte[]? values)
    {
        source ??= Array.Empty<byte>();
        values ??= Array.Empty<byte>();

        return source.Concat(values).ToArray();
    }

    internal static bool IsEqual(this byte[]? first, byte[]? second)
    {
        if (first == null || second == null || first.Length != second.Length)
        {
            return false;
        }

        return !first.Where((t, i) => t != second[i]).Any();
    }
}