using System;

namespace Atom.Design.Hosting
{
    public interface ISolution
    {
        string Name { get; }

        IProjectCollection Projects { get; }

        void Reload();

        IDocument FindDocument(string fileFullName);
    }
}
