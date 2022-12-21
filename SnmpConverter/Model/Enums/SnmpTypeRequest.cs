namespace SnmpConverter;

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