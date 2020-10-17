using System.Collections.Generic;
using System.Linq;

namespace Snmp.Serializer.HelperExtensions
{
    internal static class ByteArrayExtensions
    {
        internal static IEnumerable<byte> Append(this IEnumerable<byte> source, byte value)
        {
            var appendArray = new[] { value };

            return source is null ? appendArray : source.Concat(appendArray);
        }

        internal static IEnumerable<byte> Append(this IEnumerable<byte> source, IEnumerable<byte> value)
        {
            if (value is null || !value.Any()) return source;

            return source is null ? value : source.Concat(value);
        }

        internal static IEnumerable<byte> Prepend(this IEnumerable<byte> source, byte value)
        {
            var prependArray = new[] { value };

            return source is null ? prependArray : prependArray.Concat(source);
        }

        internal static IEnumerable<byte> Prepend(this IEnumerable<byte> source, IEnumerable<byte> value)
        {
            if (value is null || !value.Any()) return source;

            return source is null ? value : value.Concat(source);
        }
    }
}
