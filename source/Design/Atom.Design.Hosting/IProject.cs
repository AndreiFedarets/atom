using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Atom.Design.Hosting
{
    public interface IProject
    {
        Guid Id { get; }

        string Name { get; }

        string Path { get; }

        string FullName { get; }

        string OutputFilePath { get; }

        AssemblyName AssemblyName { get; }

        CodeLanguage Language { get; }

        ISolution Solution { get; }

        IDocumentCollection Documents { get; }

        IReferenceCollection References { get; }

        Compilation GetCompilation();
    }
}
