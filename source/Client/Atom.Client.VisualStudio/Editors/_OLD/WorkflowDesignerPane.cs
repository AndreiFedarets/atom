using Atom.Design;
using Atom.Design.Hosting;
using Atom.Design.Services;

namespace Atom.Client.VisualStudio.Editors
{
    public sealed class WorkflowDesignerPane : DesignerPane
    {
        public WorkflowDesignerPane(IWorkspace workspace, IDesignerSerializer designerSerializer, IViewManager viewManager)
            : base(workspace, designerSerializer, viewManager, ClientConstants.Editors.WorkflowDesignerFactoryGuid, Constants.WorkflowDesignerDocumentExtension, ClientConstants.Editors.WorkflowContentName)
        {
        }
    }
}
