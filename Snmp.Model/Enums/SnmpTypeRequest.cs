namespace Snmp.Model.Enums
{
    public enum SnmpTypeRequest : byte
    {
        GetRequest,
        SetRequest,
        GetNextRequest,
        GetBulkRequest,
        Response,
        Trap,
        InformRequest,
    }
}