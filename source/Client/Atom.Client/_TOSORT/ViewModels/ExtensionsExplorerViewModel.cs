using Atom.Design;
using Caliburn.Micro;

namespace Atom.Client.Win.ViewModels
{
    public class ExtensionsExplorerViewModel : Screen
    {
        private readonly IApplication _application;

        public ExtensionsExplorerViewModel(IApplication application)
        {
            _application = application;
        }

        public override string DisplayName
        {
            get { return "Extensions Store"; }
            set { }
        }

        public IAssemblyCollection Assemblies
        {
             get { return _application.Assemblies; }
        }
    }
}
