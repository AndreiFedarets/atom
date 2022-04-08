using Atom.Design;
using Atom.Design.Hosting;
using Atom.Design.Services;
using System;
using System.Runtime.InteropServices;

namespace Atom.Client.VisualStudio.Editors
{
    [Guid(ClientConstants.Editors.TableDesignerFactoryGuidString)]
    public sealed class TableDesignerFactory : ObjectDesignerFactory
    {
        public TableDesignerFactory(IWorkspace workspace, IDesignerSerializer designerSerializer, IViewManager viewManager)
            : base(workspace, designerSerializer, viewManager, ClientConstants.Editors.TableDesignerFactoryGuid, Constants.TableDesignerDocumentExtension, ClientConstants.Editors.TableContentName)
        {
        }
    }
}
