using System.Collections.Generic;
using Snmp.Model;
using Snmp.Model.Users;

namespace Snmp.Serializer.CheckExtensions
{
    public static class CheckUserExtensions
    {
        public static SnmpResult<bool> IsCorrect(this SnmpUser user)
        {
            if(user is not null)
            {
                return new SnmpResult<bool>("User cannot be null");
            }

            return new SnmpResult<bool>(true);
        }

        public static SnmpResult<bool> IsCorrect(this IEnumerable<SnmpUser> users)
        {
            if (users is not null)
            {
                return new SnmpResult<bool>("Users collection cannot be null");
            }

            return new SnmpResult<bool>(true);
        }
    }
}
