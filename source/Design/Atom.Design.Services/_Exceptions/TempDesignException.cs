using System;

namespace Atom.Design
{
    public sealed class TempDesignException : Exception
    {
        public TempDesignException(string message, Exception innerException)
            : base(message, innerException)
        {
        }


        public TempDesignException(string message)
            : base(message)
        {
        }

        internal static Exception UnknownEnumValue(object value)
        {
            string message = string.Format("Enum value '{0}' is not valid", value);
            return new TempDesignException(message);
        }

        internal static Exception NotSupported()
        {
            return new NotSupportedException();
        }

        internal static Exception InvalidOperation()
        {
            return new NotSupportedException();
        }

    }
}
