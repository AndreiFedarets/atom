using System.Windows.Automation;

namespace Atom.Runtime.Extension.Desktop
{
    public sealed class Window : Control, IWindow
    {
        public Window(Application application, params ControlProperty[] properties)
            : base(properties, ControlType.Window)
        {
            Application = application;
        }

        public Application Application { get; private set; }

        private WindowPattern WindowPattern => (WindowPattern)Element.GetCurrentPattern(WindowPattern.Pattern);

        public void Close()
        {
            WindowPattern.Close();
        }

        public void Maximize()
        {
            WindowPattern.SetWindowVisualState(WindowVisualState.Maximized);
        }

        public void Minimize()
        {
            WindowPattern.SetWindowVisualState(WindowVisualState.Minimized);
        }
        
    }
}
