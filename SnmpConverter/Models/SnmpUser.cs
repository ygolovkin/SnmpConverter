namespace SnmpConverter;

/// <summary>
/// SNMP user.
/// </summary>
public class SnmpUser
{
    /// <summary>
    /// User name
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// User <see cref="SnmpRights">rights</see>.
    /// </summary>
    public SnmpRights Rights { get; set; } = SnmpRights.None;

    /// <summary>
    /// User <see cref="SnmpAuthenticationType">authentication type</see>.
    /// </summary>
    public SnmpAuthenticationType AuthenticationType { get; set; } = SnmpAuthenticationType.None;

    /// <summary>
    /// User <see cref="SnmpPrivacyType">privacy type</see>.
    /// </summary>
    public SnmpPrivacyType PrivacyType { get; set; } = SnmpPrivacyType.None;

    /// <summary>
    /// User password.
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// User key.
    /// </summary>
    public string? Key { get; set; }

    /// <summary>
    /// User context name.
    /// </summary>
    public string? ContextName { get; set; }

    /// <summary>
    /// User context <see cref="SnmpEngineId">engine identifier</see>.
    /// </summary>
    public SnmpEngineId? ContextEngineId { get; set; }

    /// <summary>
    /// User <see cref="SnmpEngineId">engine identifier</see>.
    /// </summary>
    public SnmpEngineId? EngineId { get; set; }

    /// <summary>
    /// User password's hash.
    /// </summary>
    internal byte[]? HashPassword { get; set; }

    /// <summary>
    /// User key's hash.
    /// </summary>
    internal byte[]? HashKey { get; set; }
}