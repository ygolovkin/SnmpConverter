using Snmp.Model.Enums;

namespace Snmp.Model.Users
{
    public class SnmpUserAuthentication
    {
        public SnmpAuthentication Authentication { get; set; }

        public string Key { get; set; }
    }
}
