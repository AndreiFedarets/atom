using System;
using System.Collections.Generic;

namespace Atom.Design.Hosting
{
    public interface IDocumentCollection : IEnumerable<IDocument>
    {
        event EventHandler DocumentAdded;

        event EventHandler DocumentRemoved;
    }
}
