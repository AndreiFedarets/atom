using Atom.Design.Hosting;
using Atom.Design.Services;
using Layex;
using System.Windows;

namespace Atom.Client
{
    public abstract class BootstrapperBase : Layex.BootstrapperBase
    {
        private ServiceProvider _designServices;
        private ExtensionLoader _extensionLoader;

        protected BootstrapperBase()
            : base(false)
        {
            _designServices = new ServiceProvider();
        }

        protected abstract IWorkspace CreateWorkspace();

        protected override IDependencyContainer CreateDependencyContainer()
        {
            CompositeDependencyContainer container = new CompositeDependencyContainer(_designServices);
            return container;
        }

        protected override void ConfigureContainer(IDependencyContainer container)
        {
            base.ConfigureContainer(container);
            IWorkspace workspace = CreateWorkspace();
            _designServices.Initialize(workspace);
            container.RegisterInstance<IWorkspace>(workspace);
            _extensionLoader = container.Resolve<ExtensionLoader>();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);
            _extensionLoader.LoadExtensions();
        }

        //protected override void PrepareApplication()
        //{
        //    base.PrepareApplication();
        //    System.Windows.ResourceDictionary resourceDictionary = (ResourceDictionary)Application.LoadComponent(new Uri("/Atom.Client;component/Themes/Generic.xaml", UriKind.RelativeOrAbsolute));
        //    System.Windows.Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
        //}
    }
}
