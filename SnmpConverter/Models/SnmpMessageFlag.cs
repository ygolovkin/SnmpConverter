namespace SnmpConverter;

public class SnmpMessageFlag
{
    public bool AuthenticationFlag { get; set; }

    public bool PrivacyFlag { get; set; }

    public bool ReportableFlag { get; set; }

    public byte Flag
    {
        get
        {
            byte flag = 0x00;
            if (AuthenticationFlag) flag |= SnmpConstants.MessageAuthenticationFlag;
            if (PrivacyFlag) flag |= SnmpConstants.MessagePrivacyFlag;
            if (ReportableFlag) flag |= SnmpConstants.MessagePrivacyFlag;
            return flag;
        }
        set
        {
            AuthenticationFlag = (value & SnmpConstants.MessageAuthenticationFlag) != 0;
            PrivacyFlag = (value & SnmpConstants.MessagePrivacyFlag) != 0;
            ReportableFlag = (value & SnmpConstants.MessageReportableFlag) != 0;
        }
    }

    public SnmpMessageFlag(byte flag)
    {
        Flag = flag;
    }
}