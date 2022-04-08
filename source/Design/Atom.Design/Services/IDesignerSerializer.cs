using Atom.Design.Hosting;

namespace Atom.Design.Services
{
    public interface IDesignerSerializer
    {
        bool CanRead(IDocument document);

        IObjectDesigner Read(IDocument document);
        
        bool Write(IObjectDesigner designer);
    }
}
