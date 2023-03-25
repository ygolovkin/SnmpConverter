using System.Collections.Generic;

namespace SnmpConverter;

/// <summary>
/// SNMP base packet.
/// </summary>
public abstract class SnmpBasePacket
{
    /// <summary>
    /// SNMP <see cref="SnmpVersion">version</see>.
    /// </summary>
    public SnmpVersion Version { get; protected set; }

    /// <summary>
    /// SNMP <see cref="SnmpPduType">PDU type</see>.
    /// </summary>
    public SnmpPduType PduType { get; set; }

    /// <summary>
    /// SNMP request identifier.
    /// </summary>
    public int RequestId { get; set; }

    /// <summary>
    /// SNMP <see cref="SnmpErrorStatus">error status</see>.
    /// </summary>
    public SnmpErrorStatus ErrorStatus { get; set; }

    /// <summary>
    /// SNMP error index.
    /// </summary>
    public int ErrorIndex { get; set; }

    /// <summary>
    /// SNMP collection of <see cref="SnmpVariableBinding">variable vindings</see>.
    /// </summary>
    public ICollection<SnmpVariableBinding> VariableBindings { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SnmpBasePacket"/> class.
    /// </summary>
    protected SnmpBasePacket()
    {
        VariableBindings = new List<SnmpVariableBinding>();
    }
}