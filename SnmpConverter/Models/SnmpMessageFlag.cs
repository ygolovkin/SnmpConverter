namespace SnmpConverter;

/// <summary>
/// SNMP message flag.
/// </summary>
public class SnmpMessageFlag
{
    /// <summary>
    /// Authentication flag.
    /// </summary>
    public bool AuthenticationFlag { get; set; }

    /// <summary>
    /// Privacy flag.
    /// </summary>
    public bool PrivacyFlag { get; set; }

    /// <summary>
    /// Reportable flag.
    /// </summary>
    public bool ReportableFlag { get; set; }

    /// <summary>
    /// Get flag.
    /// </summary>
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

    /// <summary>
    /// Initializes a new instance of the <see cref="SnmpMessageFlag"/> class.
    /// </summary>
    /// <param name="flag">Default value.</param>
    public SnmpMessageFlag(byte flag)
    {
        Flag = flag;
    }
}