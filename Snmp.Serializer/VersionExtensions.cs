using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snmp.Model;
using Snmp.Model.Enums;

namespace Snmp.Serializer
{
    internal static class VersionExtensions
    {
        internal static SnmpResult<SnmpVersion> ToVersion(this byte[] source, ref int offset)
        {
            var intResult = source.ToInt(ref offset);
            if(intResult.HasError) return new SnmpResult<SnmpVersion>(intResult.Error);

            return (byte) intResult.Value switch
            {
                0x01 => new SnmpResult<SnmpVersion>(SnmpVersion.v2c),
                _ => new SnmpResult<SnmpVersion>("Unsupported version")
            };
        }

        internal static SnmpResult<byte[]> ToByteArray(this SnmpVersion version)
        {
            return ((int) version).ToByteArray();
        }
    }
}
