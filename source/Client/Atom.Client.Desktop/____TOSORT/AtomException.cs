using System;

namespace Atom
{
    public class AtomException : Exception
    {
        public AtomException()
        {
        }

        public AtomException(string message)
            : base(message)
        {
        }

        public AtomException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
