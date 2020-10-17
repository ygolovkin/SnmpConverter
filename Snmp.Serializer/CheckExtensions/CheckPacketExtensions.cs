using Snmp.Model.Exceptions;
using Snmp.Model.Packet;

namespace Snmp.Serializer.CheckExtensions
{
    public static class CheckPacketExtensions
    {
        public static void GeneralCheck(this SnmpBasePacket packet)
        {
            if (packet is null)
            {
                throw new SnmpException("Packet can't be null.");
            }
        }
    }
}
