using System.Windows.Automation;

namespace Atom.Runtime.Extension.Desktop
{
    public interface IControl
    {
        AutomationElement Element { get; }

        bool IsDisplayed { get; }

        bool IsEnabled { get; }

        void AttachTo(IControl parent);
    }
}
