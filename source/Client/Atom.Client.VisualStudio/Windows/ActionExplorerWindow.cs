using Atom.Client.Views;
using System.Runtime.InteropServices;

namespace Atom.Client.VisualStudio.Windows
{
    [Guid("5754FBC5-1F17-4E34-BC2D-A9AFA7B30D35")]
    public class ActionExplorerWindow : ToolWindowBase
    {
        public ActionExplorerWindow()
        {
        }

        protected override void InitializeContent()
        {
            //BitmapImageMoniker = Microsoft.VisualStudio.Imaging.KnownMonikers.Search;
            //View view = Bootstrapper.ViewsManager.ResolveActionExplorerView();
            //InternalContent = view;
            //Caption = view.DisplayName;
        }
    }
}
