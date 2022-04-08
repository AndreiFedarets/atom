using Layex.Views;

namespace Atom.Client.VisualStudio.Windows
{
    public class DialogWindow : Microsoft.VisualStudio.PlatformUI.DialogWindow
    {
        public DialogWindow(View view)
        {
            Content = view;
            Title = view.DisplayName;
            Height = 600;
            Width = 800;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }
    }
}
