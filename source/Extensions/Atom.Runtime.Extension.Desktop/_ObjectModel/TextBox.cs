using System.Windows.Automation;

namespace Atom.Runtime.Extension.Desktop
{
    public sealed class TextBox : Control, ITextBox
    {
        public TextBox(params ControlProperty[] properties)
            : base(properties, ControlType.Text)
        {
        }

        public string Value
        {
            get { return TextPattern.DocumentRange.GetText(int.MaxValue); }
        }

        private TextPattern TextPattern
        {
            get { return (TextPattern)Element.GetCurrentPattern(TextPattern.Pattern); }
        }
    }
}
