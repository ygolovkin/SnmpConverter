using System.Linq;

namespace Snmp.Serializer.HelperExtensions
{
    public static class ByteArrayExtensions
    {
        public static byte[] Append(this byte[] source, byte value)
        {
            var appendArray = new[] { value };

            return source is null ? appendArray : source.Concat(appendArray).ToArray();
        }

        public static byte[] Prepend(this byte[] source, byte value)
        {
            var prependArray = new[] { value };

            return source is null ? prependArray : prependArray.Concat(source).ToArray();
        }
    }
}
