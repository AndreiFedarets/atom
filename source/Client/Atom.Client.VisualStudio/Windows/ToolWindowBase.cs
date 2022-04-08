using Microsoft.VisualStudio.Shell;
using System.Windows;
using System.Windows.Controls;

namespace Atom.Client.VisualStudio.Windows
{
    public abstract class ToolWindowBase : ToolWindowPane
    {
        public ToolWindowBase()
            : base(null)
        {
            Content = new UserControl();
        }

        protected FrameworkElement InternalContent
        {
            get { return (FrameworkElement)((UserControl)Content).Content; }
            set { ((UserControl)Content).Content = value; }
        }
        
        public override void OnToolWindowCreated()
        {
            base.OnToolWindowCreated();
            InitializeContent();
        }

        public override void OnToolBarAdded()
        {
            base.OnToolBarAdded();
        }

        protected abstract void InitializeContent();
    }
}
