using System.Windows.Automation;

namespace Atom.Runtime.Extension.Desktop
{
    public sealed class Button : Control, IButton
    {
        public Button(params ControlProperty[] properties)
            : base(properties, ControlType.Button)
        {
        }

        public void Invoke()
        {
            InvokePattern pattern = (InvokePattern)Element.GetCurrentPattern(InvokePattern.Pattern);
            pattern.Invoke();
        }
    }
}
