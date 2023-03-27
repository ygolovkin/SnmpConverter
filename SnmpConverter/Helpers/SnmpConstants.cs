namespace SnmpConverter;

internal static class SnmpConstants
{
    internal const byte HighByte = 0x80;

    internal const byte LastByte = 0xFF;

    internal const int MinBufferSize = 484;
    
    internal const byte MessageAuthenticationFlag = 0x01;

    internal const byte MessagePrivacyFlag = 0x02;

    internal const byte MessageReportableFlag = 0x04;

    internal const byte Sequence = 0x30;
}