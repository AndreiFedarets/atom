using Atom.Design;
using Atom.Design.Hosting;
using Atom.Design.Services;
using System;
using System.Runtime.InteropServices;

namespace Atom.Client.VisualStudio.Editors
{
    [Guid(ClientConstants.Editors.ActionDesignerFactoryGuidString)]
    public sealed class ActionDesignerFactory : ObjectDesignerFactory
    {
        public ActionDesignerFactory(IWorkspace workspace, IDesignerSerializer designerSerializer, IViewManager viewManager)
            : base(workspace, designerSerializer, viewManager, ClientConstants.Editors.ActionDesignerFactoryGuid, Constants.ActionDesignerDocumentExtension, ClientConstants.Editors.ActionContentName)
        {
        }
    }
}
