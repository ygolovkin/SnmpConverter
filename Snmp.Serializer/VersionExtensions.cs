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
            var intResult = source.GetInt(ref offset);
            if(intResult.HasError) return new SnmpResult<SnmpVersion>(intResult.Error);
            if (intResult.Value < 0 || intResult.Value > 2) return new SnmpResult<SnmpVersion>("Incorrect value of version");

            return (byte) intResult.Value switch
            {
                0x01 => new SnmpResult<SnmpVersion>(SnmpVersion.v2c),
                _ => new SnmpResult<SnmpVersion>("Unsupported version")
            };
        }

        internal static SnmpResult<byte[]> ToByteArray(this SnmpVersion source)
        {
            var intResult = ((int) source).ToByteArray();
            return intResult.HasError ? new SnmpResult<byte[]>(intResult.Error) : intResult;
        }
    }
}
