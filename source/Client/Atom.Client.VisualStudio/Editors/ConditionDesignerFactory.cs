using Atom.Design;
using Atom.Design.Hosting;
using Atom.Design.Services;
using System;
using System.Runtime.InteropServices;

namespace Atom.Client.VisualStudio.Editors
{
    [Guid(ClientConstants.Editors.ConditionDesignerFactoryGuidString)]
    public sealed class ConditionDesignerFactory : ObjectDesignerFactory
    {
        public ConditionDesignerFactory(IWorkspace workspace, IDesignerSerializer designerSerializer, IViewManager viewManager)
            : base(workspace, designerSerializer, viewManager, ClientConstants.Editors.ActionDesignerFactoryGuid, Constants.ActionDesignerDocumentExtension, ClientConstants.Editors.ActionContentName)
        {
        }
    }
}
