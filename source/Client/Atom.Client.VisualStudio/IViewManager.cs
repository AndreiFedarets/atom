using Atom.Design;

namespace Atom.Client.VisualStudio
{
    public interface IViewManager
    {
        Views.DesignerView CreateDesignerView(IObjectDesigner designer);
    }
}
