using System;

namespace Atom.Client
{
    public interface INotifyDocumentChanged
    {
        event EventHandler DocumentChanged;
    }
}
