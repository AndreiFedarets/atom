using Atom.Runtime;

namespace Atom.Runtime.Extension.Desktop
{
    public static class TextBoxActions
    {
        [ActionMethod("Assert {textbox} text in {window} equals to 4")]
        public static void AssertTextBoxValueEqualsTo(ITextBox textbox, IWindow window)
        {
            textbox.AttachTo(window);
            string text = textbox.Value;
            bool result = string.Equals(text, "5");
        }
    }
}
