namespace SnmpConverter;

/// <summary>
/// SNMP Error status
/// </summary>
public enum SnmpErrorStatus : byte
{
    /// <summary>
    /// No error
    /// </summary>
    NoError = 0x00,

    /// <summary>
    /// Too big request
    /// </summary>
    TooBig,
    NoSuchName,
    BadValue,
    ReadOnly,
    GenErr,
    NoAccess,
    WrongType,
    WrongLength,
    WrongEncoding,
    WrongValue,
    NoCreation,
    InconsistentValue,
    ResourceUnavailable,
    CommitFailed,
    UndoFailed,
    AuthorizationError,
    NotWritable,
    InconsistentName
}