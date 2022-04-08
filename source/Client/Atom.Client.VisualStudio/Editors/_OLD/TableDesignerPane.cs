using Atom.Design;
using Atom.Design.Hosting;
using Atom.Design.Services;

namespace Atom.Client.VisualStudio.Editors
{
    public sealed class TableDesignerPane : DesignerPane
    {
        public TableDesignerPane(IWorkspace workspace, IDesignerSerializer designerSerializer, IViewManager viewManager)
            : base(workspace, designerSerializer, viewManager, ClientConstants.Editors.DataTableDesignerFactoryGuid, Constants.TableDesignerDocumentExtension, ClientConstants.Editors.DataTableContentName)
        {
        }
    }
}
