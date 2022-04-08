using Atom.Runtime;

namespace Atom.Runtime.Extension.Desktop
{
    public static class ButtonActions
    {
        [ActionMethod("Click {button} in {window}")]
        public static void ClickButton(IButton button, IWindow window)
        {
            button.AttachTo(window);
            button.Invoke();
        }
    }
}
