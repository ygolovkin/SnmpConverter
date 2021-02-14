namespace Snmp.Model.Users
{
    public class SnmpUser
    {
        public string Name { get; set; }

        public SnmpUserAuthentication Authentication { get; set; }

        public SnmpUserPrivacy Privacy { get; set; }
    }
}
