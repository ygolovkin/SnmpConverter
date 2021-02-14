using Snmp.Model;
using Snmp.Model.Exceptions;
using Snmp.Model.Packet;

namespace Snmp.Serializer.CheckExtensions
{
    internal static class CheckPacketExtensions
    {
        internal static SnmpResult<bool> IsCorrect(this SnmpBasePacket packet)
        {
            if (packet is null)
            {
                return new SnmpResult<bool>("Packet cannot be null");
            }

            return new SnmpResult<bool>(true);
        }
    }
}
