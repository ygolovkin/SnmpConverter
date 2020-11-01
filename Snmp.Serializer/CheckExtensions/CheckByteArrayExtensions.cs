using Snmp.Model.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace Snmp.Serializer.CheckExtensions
{
    public static class CheckByteArrayExtensions
    {
        public static void CheckLength(this IEnumerable<byte> source)
        {
            if(source is null)
            {
                throw new SnmpException("Array cannot be null.");
            }

            if(source.Count() < 2)
            {
                throw new SnmpException("Array too small.");
            }
        }

        public static void CheckLength(this IEnumerable<byte> source, int offset)
        {
            source.CheckLength();

            if (source.Count() == offset)
            {
                throw new SnmpException("Array too small.");
            }
        }
    }
}
