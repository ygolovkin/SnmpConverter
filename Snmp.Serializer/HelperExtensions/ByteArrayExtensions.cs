using System.Linq;

namespace Snmp.Serializer.HelperExtensions
{
    internal static class ByteArrayExtensions
    {
        internal static byte[] Append(this byte[] source, byte value)
        {
            var appendArray = new[] { value };

            return source is null ? appendArray : source.Concat(appendArray).ToArray();
        }

        internal static byte[] Prepend(this byte[] source, byte value)
        {
            var prependArray = new[] { value };

            return source is null ? prependArray : prependArray.Concat(source).ToArray();
        }
    }
}
