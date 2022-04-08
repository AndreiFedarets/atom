using Atom.Runtime;
using System.Windows.Forms;

namespace Atom.Runtime.Extension.Common
{
    public static class MessageBoxActions
    {
        [ActionMethod("Show MessageBox with {text}")]
        public static void ShowMessageBox([ActionParameter("text")] string text)
        {
            MessageBox.Show(text);
        }
    }
}
