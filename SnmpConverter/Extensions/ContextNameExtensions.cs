namespace SnmpConverter;

internal static class ContextNameExtensions
{
    internal static string ToContextName(this byte[] source, SnmpEngineId engineId, SnmpUser user, ref int offset)
    {
        var contextName = source.ToString(ref offset);

        if(!engineId.IsEmpty && contextName != user.ContextName)
        {
            throw new SnmpException("Incorrect user's Context Name.");
        }

        return contextName;
    }

    internal static byte[] ToContextNameArray(this string? contextName)
    {
        return contextName.ToStringArray();
    }
}