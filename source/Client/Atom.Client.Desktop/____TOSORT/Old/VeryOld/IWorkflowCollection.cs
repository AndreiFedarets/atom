using System.Collections.Generic;

namespace Atom
{
    public interface IWorkflowCollection : IEnumerable<IWorkflow>
    {
        IWorkflow Create(string name, string description);
    }
}
