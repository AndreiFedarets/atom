using Atom.Design.Hosting;
using System.Collections.Generic;

namespace Atom.Design.Reflection.Code.Services
{
    public interface ICodeParser
    {
        IList<T> Parse<T>(IProject project);

        IList<T> Parse<T>(IDocument document);
    }
}
