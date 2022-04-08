using System.Collections.Generic;
using System.IO;

namespace Atom
{
    public interface IDirectories
    {
        IEnumerable<DirectoryInfo> ModulesDirectories { get; }

        DirectoryInfo SolutionsDefaultDirectory { get; }
    }
}
