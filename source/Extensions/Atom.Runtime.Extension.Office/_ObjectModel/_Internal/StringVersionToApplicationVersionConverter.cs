using System;

namespace Atom.Office
{
    internal static class StringVersionToApplicationVersionConverter
    {
        public static ApplicationVersion Convert(string version)
        {
            if (version.Equals("14.0", StringComparison.OrdinalIgnoreCase))
            {
                return ApplicationVersion.Office2010;
            }
            else if (version.Equals("15.0", StringComparison.OrdinalIgnoreCase))
            {
                return ApplicationVersion.Office2013;
            }
            else if (version.Equals("16.0", StringComparison.OrdinalIgnoreCase))
            {
                return ApplicationVersion.Office2016;
            }
            //TODO: how to handle Office 365 ?
            //else if (version.Equals("", StringComparison.OrdinalIgnoreCase))
            //{
            //    return ApplicationVersion.Office365;
            //}
            return ApplicationVersion.Unknown;
        }

        public static string Convert(ApplicationVersion version)
        {
            switch (version)
            {
                case ApplicationVersion.Office2010:
                    return "14.0";
                case ApplicationVersion.Office2013:
                    return "15.0";
                case ApplicationVersion.Office2016:
                    return "16.0";
                //TODO: how to handle Office 365 ?
                //case ApplicationVersion.Office365:
                //    return
                default:
                    return string.Empty;
            }
        }
    }
}
