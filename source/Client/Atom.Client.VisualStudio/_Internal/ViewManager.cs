using Atom.Design;
using Atom.Design.Services;
using Layex;

namespace Atom.Client.VisualStudio
{
    internal class ViewManager : IViewManager
    {
        private readonly Layex.ViewModels.IViewModelManager _viewModelManager;
        private readonly IDependencyContainer _container;

        public ViewManager(Layex.ViewModels.IViewModelManager viewModelManager, IDependencyContainer container)
        {
            _viewModelManager = viewModelManager;
            _container = container;
        }

        public Views.DesignerView CreateDesignerView(IObjectDesigner designer)
        {
            IDependencyContainer childContainer = _container.CreateChildContainer();
            childContainer.RegisterInstance<IObjectDesigner>(designer);
            ViewModels.DesignerViewModel viewModel = childContainer.Resolve<ViewModels.DesignerViewModel>();
            return Layex.Views.ViewManager.LocateViewForViewModel(viewModel) as Views.DesignerView;
        }
    }
}
