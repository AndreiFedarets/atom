using System;

namespace Atom.Design
{
    internal static class Fail
    {
        public static class Design
        {
            public static Exception TempException()
            {
                throw new NotImplementedException();
            }
        }

        public static class Internal
        {
            internal static Exception ArgumentNullOrEmpty(string p)
            {
                throw new NotImplementedException();
            }
        }

        public static Exception NotImplemented()
        {
            return new NotImplementedException();
        }

        public static Exception FileIsNotCorrectAssembly(string fileFullName, Exception exception = null)
        {
            throw new NotImplementedException();
        }

        public static Exception MissingActionType(Guid actionTypeUid)
        {
            throw new NotImplementedException();
        }
    }
}
