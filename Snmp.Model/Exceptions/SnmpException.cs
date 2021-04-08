﻿using System;

namespace Snmp.Model.Exceptions
{
    public class SnmpException : Exception
    {
        public SnmpException() : base() { }

        public SnmpException(string message) : base(message) { }

        public SnmpException(string message, Exception innerException) : base(message, innerException) { }
    }
}
