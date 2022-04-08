using System;

namespace Atom.Office
{
    internal static class ApplicationTypeToProcessNameConverter
    {
        private const string ExcelProcessName = "EXCEL";
        private const string WordProcessName = "WINWORD";
        private const string PowerPointProcessName = "POWERPNT";

        public static ApplicationType Convert(string processName)
        {
            if (processName.Equals(ExcelProcessName, StringComparison.InvariantCultureIgnoreCase))
            {
                return ApplicationType.Excel;
            }
            if (processName.Equals(PowerPointProcessName, StringComparison.InvariantCultureIgnoreCase))
            {
                return ApplicationType.PowerPoint;
            }
            if (processName.Equals(WordProcessName, StringComparison.InvariantCultureIgnoreCase))
            {
                return ApplicationType.Word;
            }
            return ApplicationType.Unknown;
        }

        public static string Convert(ApplicationType applicationType)
        {
            switch (applicationType)
            {
                case ApplicationType.Excel:
                    return ExcelProcessName;
                case ApplicationType.PowerPoint:
                    return PowerPointProcessName;
                case ApplicationType.Word:
                    return WordProcessName;
                default:
                    return string.Empty;
            }
        }
    }
}
