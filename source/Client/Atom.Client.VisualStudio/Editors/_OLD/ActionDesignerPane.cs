using Atom.Design;
using Atom.Design.Hosting;
using Atom.Design.Services;

namespace Atom.Client.VisualStudio.Editors
{
    public sealed class ActionDesignerPane : DesignerPane
    {
        public ActionDesignerPane(IWorkspace workspace, IDesignerSerializer designerSerializer, IViewManager viewManager)
            : base(workspace, designerSerializer, viewManager, ClientConstants.Editors.ActionDesignerFactoryGuid, Constants.ActionDesignerDocumentExtension, ClientConstants.Editors.ActionContentName)
        {
            
        }
    }
}
