namespace SnmpConverter;

public class SnmpUser
{
    public string? Username { get; set; }

    public SnmpRights Rights { get; set; } = SnmpRights.None;

    public SnmpAuthenticationType AuthenticationType { get; set; } = SnmpAuthenticationType.None;

    public SnmpPrivacyType PrivacyType { get; set; } = SnmpPrivacyType.None;

    public string? Password { get; set; }

    public string? Key { get; set; }

    public string? ContextName { get; set; }

    public SnmpEngineId? ContextEngineId { get; set; }

    public SnmpEngineId? EngineId { get; set; }
    
    internal byte[]? HashPassword { get; set; }

    internal byte[]? HashKey { get; set; }
}