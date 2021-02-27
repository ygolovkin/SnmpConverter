using Snmp.Model;
using Snmp.Model.Packet;

namespace Snmp.Serializer.CheckExtensions
{
    public static class CheckEngineIdExtensions
    {
        public static SnmpResult<bool> IsCorrect(this SnmpEngineId engineId)
        {
            if (engineId is null) return new SnmpResult<bool>("EngineId cannot be null");
            if (engineId.Length < 5 || engineId.Length > 32) return new SnmpResult<bool>("Incorrect EngineId length");

            var engine2 = new SnmpEngineId();
            var r = engine2 == engineId;

            return new SnmpResult<bool>(true);
        }
    }
}
