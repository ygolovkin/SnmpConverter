namespace SnmpConverter.Models.Enums;

/// <summary>
/// SNMP Authentication Type
/// </summary>
public enum SnmpAuthenticationType
{
    /// <summary>
    /// None of authentication type
    /// </summary>
    None,

    /// <summary>
    /// SHA1 authentication type
    /// </summary>
    [Obsolete("Use SHA256 or SHA384")]
    SHA1,

    /// <summary>
    /// MD5 authentication type
    /// </summary>
    [Obsolete("Use SHA256 or SHA384")]
    MD5,

    /// <summary>
    /// SHA256 authentication type
    /// </summary>
    SHA256,

    /// <summary>
    /// SHA384 authentication type
    /// </summary>
    SHA384
}