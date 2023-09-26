using System;

namespace SnmpConverter;

internal static class AuthenticationExtensions
{
    internal static byte[] ToAuthenticationParameter(this byte[] source, SnmpEngineId engineId, SnmpUser user, ref int offset)
    {
        var length = source.ToLength(ref offset, SnmpValueType.OctetString, x => x != 12, "Incorrect authentication parameter's length.");

        var parameter = new byte[length];
        var startPosition = offset;
        Buffer.BlockCopy(source, offset, parameter, 0, length);
        offset += length;

        if (!engineId.IsEmpty && user.AuthenticationType != SnmpAuthenticationType.None)
        {
            var buffer = new byte[source.Length];
            var emptyHash = new byte[12];
            Buffer.BlockCopy(buffer, 0, buffer, 0, buffer.Length);
            Buffer.BlockCopy(emptyHash, 0, buffer, startPosition, emptyHash.Length);
            var hash = buffer.GetHash(user);

            if (!parameter.IsEqual(hash))
            {
                throw new SnmpException("Incorrect password.");
            }
        }

        return parameter;
    }

    internal static byte[] ToAuthenticationParameterArray(this byte[] authenticationParameter)
    {
        return authenticationParameter.ToArrayWithLength();
    }
}