using Atom.Client.VisualStudio.Editors;
using Atom.Design.Hosting;
using Atom.Design.Services;
using Layex;
using Microsoft;
using Microsoft.VisualStudio.ComponentModelHost;
using System.Windows;

namespace Atom.Client.VisualStudio
{
    public sealed class Bootstrapper : BootstrapperBase
    {
        private IVisualStudioPackage _package;
        private IMenuCommandManager _menuManager;

        public void Initialize(IVisualStudioPackage package)
        {
            _package = package;
            Initialize();
        }

        protected override IWorkspace CreateWorkspace()
        {
            IComponentModel componentModel = (IComponentModel)_package.GetService(typeof(SComponentModel));
            Assumes.Present(componentModel);
            Microsoft.VisualStudio.LanguageServices.VisualStudioWorkspace nativeWorkspace = componentModel.GetService<Microsoft.VisualStudio.LanguageServices.VisualStudioWorkspace>();
            EnvDTE.DTE dte = _package.GetService<EnvDTE.DTE>();
            VisualStudioWorkspace workspace = new VisualStudioWorkspace(nativeWorkspace, dte);
            return workspace;
        }

        protected override void ConfigureContainer(IDependencyContainer container)
        {
            base.ConfigureContainer(container);
            container.RegisterType<IMenuCommandManager, MenuCommandManager>(true);
            container.RegisterType<IViewManager, ViewManager>(true);

            IWorkspace workspace = container.Resolve<IWorkspace>();
            IWorkflowDebugger debugger = container.Resolve<IWorkflowDebugger>();
            _menuManager = new MenuCommandManager(_package, workspace, debugger);

            ActionDesignerFactory actionDesignerFactory = container.Resolve<ActionDesignerFactory>();
            _package.RegisterEditorFactory(actionDesignerFactory);
            WorkflowDesignerFactory workflowDesignerFactory = container.Resolve<WorkflowDesignerFactory>();
            _package.RegisterEditorFactory(workflowDesignerFactory);
            TableDesignerFactory dataTableDesignerFactory = container.Resolve<TableDesignerFactory>();
            _package.RegisterEditorFactory(dataTableDesignerFactory);
            OnStartup(null, null);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);
            _menuManager.Initialize();

        }
    }
}
