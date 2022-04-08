using System;

namespace Atom.Office
{
    public class OfficeApplicationRunException : ApplicationException
    {
        public OfficeApplicationRunException(ApplicationType applicationType, ApplicationVersion applicationVersion,
                Exception innerException) : base("Aplication cannot be started", innerException)
        {
            ApplicationType = applicationType;
            ApplicationVersion = applicationVersion;
        }

        public ApplicationType ApplicationType { get; private set; }

        public ApplicationVersion ApplicationVersion { get; private set; }
    }
}
