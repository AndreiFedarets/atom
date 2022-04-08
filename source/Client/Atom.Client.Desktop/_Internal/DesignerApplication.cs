using Atom.Design;
using Atom.Design.Hosting;
using System;
using System.Reflection;

namespace Atom.Client.Desktop
{
    internal sealed class DesignerApplication : Client.DesignerApplication
    {
        private IWorkspace _workspace;

        public DesignerApplication()
        {

        }

        public override IProject GetSelectedProject()
        {
            throw new NotImplementedException();
        }

        protected override void OnStartup()
        {
            IWindowsManager windowsManager = Container.Resolve<IWindowsManager>();
            windowsManager.ShowMain();
            base.OnStartup();
        }

        public override void OpenSolution(string fileFullName)
        {
            _workspace.OpenSolution(fileFullName);
            RaiseSolutionOpened();
        }

        public override void CloseSolution()
        {
            _workspace.CloseSolution();
            RaiseSolutionClosed();
        }

        protected override void Configure()
        {
            _workspace = WorkspaceFactory.Create();
            Container.RegisterIntance<IWorkspace>(_workspace);
            Container.RegisterIntance<IMSBuildWorkspace>(_workspace);

            Container.RegisterType<IViewsManager, ViewsManager>();
            Container.RegisterType<IWindowsManager, WindowsManager>();
            ContainerConfigurator.Configure(Container);
        }

        public override void Attach(int processId)
        {
            throw new NotImplementedException();
        }
    }
}
