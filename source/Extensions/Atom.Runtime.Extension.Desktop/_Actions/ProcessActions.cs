using System.Diagnostics;

namespace Atom.Runtime.Extension.Desktop
{
    public static class ProcessActions
    {
        [ActionMethod("Open {window} application")]
        public static void StartWindowApplication(IWindow window)
        {
            Process process = window.Application.Start();
            process.WaitForInputIdle();
        }
    }
}
