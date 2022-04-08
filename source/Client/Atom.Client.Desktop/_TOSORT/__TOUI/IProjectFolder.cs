using System.Collections.Generic;

namespace Atom.Design
{
    public interface IProjectFolder : IProjectItem, IReadOnlyCollection<IProjectItem>
    {
        //void AddFile(...);

        //void AddFolder(...);

        void Sort();
    }
}
