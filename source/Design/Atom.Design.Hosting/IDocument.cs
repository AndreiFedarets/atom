using System;
using System.Collections.Generic;

namespace Atom.Design.Hosting
{
    public interface IDocument
    {
        Guid Id { get; }

        string Namespace { get; }

        string Name { get; }

        string Path { get; }

        string FullName { get; }

        DocumentType DocumentType { get; }

        IProject Project { get; }

        IDocument Designer { get; }

        IReadOnlyList<string> Folders { get; }

        event EventHandler DocumentChanged;

        string GetSourceText();

        object GetSyntaxRoot();
    }
}
