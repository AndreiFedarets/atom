using Atom.Design;
using Atom.Design.Hosting;
using Atom.Design.Services;
using System;
using System.Runtime.InteropServices;

namespace Atom.Client.VisualStudio.Editors
{
    [Guid(ClientConstants.Editors.WorkflowDesignerFactoryGuidString)]
    public sealed class WorkflowDesignerFactory : ObjectDesignerFactory
    {
        public WorkflowDesignerFactory(IWorkspace workspace, IDesignerSerializer designerSerializer, IViewManager viewManager)
            : base(workspace, designerSerializer, viewManager, ClientConstants.Editors.WorkflowDesignerFactoryGuid, Constants.WorkflowDesignerDocumentExtension, ClientConstants.Editors.WorkflowContentName)
        {
        }
    }
}
