using Atom.Runtime;
using System.Threading;

namespace Atom.Runtime.Extension.Desktop
{
    public static class WindowActions
    {
        [ActionMethod("Close {window}")]
        public static void ClickButton(IWindow window)
        {
            window.Close();
        }

        [ActionMethod("Wait for {window} opened")]
        public static void WaitForWindowOpened(IWindow window)
        {
            while (window.Element == null)
            {
                Thread.Sleep(300);
            }
        }

        [ActionMethod("Rename {sourceWindow} to {targetWindow}")]
        public static void SwitchWindow(IWindow sourceWindow, out IWindow targetWindow)
        {
            targetWindow = sourceWindow;
        }
    }
}
