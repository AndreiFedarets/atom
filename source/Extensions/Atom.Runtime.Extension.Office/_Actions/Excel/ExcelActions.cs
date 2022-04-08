using Atom.Office.Excel;
using Atom.Runtime;

namespace Atom.Office.Actions.Excel
{
    public static class ExcelActions
    {
        [ActionMethod("Start Excel {application} 2010 ")]
        public static void StartExcel2010([ActionParameter("application")] out IExcelApplication application)
        {
            IApplicationProvider applicationProvider = new ApplicationProvider();
            application = applicationProvider.Start(ApplicationVersion.Office2010);
        }

        [ActionMethod("Close Excel {application}")]
        public static void CloseExcel([ActionParameter("application")] IExcelApplication application)
        {
            IApplicationProvider applicationProvider = new ApplicationProvider();
            applicationProvider.Close(application);
        }
    }
}
