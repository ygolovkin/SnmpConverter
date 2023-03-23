namespace SnmpConverter;

/// <summary>
/// SNMP Privacy Type
/// </summary>
public enum SnmpPrivacyType
{
    /// <summary>
    /// None of privacy type
    /// </summary>
    None,

    /// <summary>
    /// Des privacy type
    /// </summary>
    Des,

    /// <summary>
    /// 3Des privacy type
    /// </summary>
    TripleDes,

    /// <summary>
    /// Aes128 privacy type
    /// </summary>
    Aes128,

    /// <summary>
    /// Aes192 privacy type
    /// </summary>
    Aes192,

    /// <summary>
    /// Aes256 privacy type
    /// </summary>
    Aes256
}