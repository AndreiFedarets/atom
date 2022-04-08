using System.Windows;
using Atom.Design.Hosting;
using Layex;

namespace Atom.Client.Desktop
{
    internal sealed class Bootstrapper : BootstrapperBase
    {
        private IDependencyContainer _container;

        protected override IWorkspace CreateWorkspace()
        {
            Microsoft.CodeAnalysis.MSBuild.MSBuildWorkspace nativeWofkspace = Microsoft.CodeAnalysis.MSBuild.MSBuildWorkspace.Create();
            IWorkspace workspace = new MSBuildWorkspace(nativeWofkspace);
            return workspace;
        }

        protected override void ConfigureContainer(IDependencyContainer container)
        {
            _container = container;
            base.ConfigureContainer(container);
            container.RegisterType<IViewManager, ViewManager>(true);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);
            KeyboardCommands keyboardCommands = _container.Resolve<KeyboardCommands>();
            keyboardCommands.Initialize();
        }
    }
}
