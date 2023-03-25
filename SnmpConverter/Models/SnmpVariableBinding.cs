namespace SnmpConverter;

/// <summary>
/// SNMP variable binding.
/// </summary>
public class SnmpVariableBinding
{
    /// <summary>
    /// SNMP <see cref="SnmpOid">object identifier</see>.
    /// </summary>
    public SnmpOid? Oid { get; set; }

    /// <summary>
    /// SNMP <see cref="SnmpValueType">value type</see>.
    /// </summary>
    public SnmpValueType Type { get; set; }

    /// <summary>
    /// Value
    /// </summary>
    public byte[]? Value { get; set; }
}