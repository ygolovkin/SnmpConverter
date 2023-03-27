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

    /// <summary>
    /// Requested name doesn't exist
    /// </summary>
    NoSuchName,

    /// <summary>
    /// Bad value supplied
    /// </summary>
    BadValue,

    /// <summary>
    /// Oid is read only
    /// </summary>
    ReadOnly,

    /// <summary>
    /// General error
    /// </summary>
    GeneralError,

    /// <summary>
    /// No access
    /// </summary>
    NoAccess,

    /// <summary>
    /// Wrong type
    /// </summary>
    WrongType,

    /// <summary>
    /// Wrong length
    /// </summary>
    WrongLength,

    /// <summary>
    /// Wrong encoding
    /// </summary>
    WrongEncoding,

    /// <summary>
    /// Wrong value
    /// </summary>
    WrongValue,

    /// <summary>
    /// No creation
    /// </summary>
    NoCreation,

    /// <summary>
    /// Inconsistent value
    /// </summary>
    InconsistentValue,

    /// <summary>
    /// Resource is unavailable
    /// </summary>
    ResourceUnavailable,

    /// <summary>
    /// Commit failed
    /// </summary>
    CommitFailed,

    /// <summary>
    /// Undo failed
    /// </summary>
    UndoFailed,

    /// <summary>
    /// Authorization error
    /// </summary>
    AuthorizationError,

    /// <summary>
    /// Not writable
    /// </summary>
    NotWritable,

    /// <summary>
    /// Inconsistent name
    /// </summary>
    InconsistentName
}