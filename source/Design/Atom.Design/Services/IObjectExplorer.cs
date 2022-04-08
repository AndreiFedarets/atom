using Atom.Design.Hosting;
using Atom.Design.Reflection;
using System.Collections.Generic;

namespace Atom.Design.Services
{
    public interface IObjectExplorer
    {
        List<IAction> GetAvailableActions(IProject project);

        List<ITable> GetAvailableTables(IProject project);
    }
}
