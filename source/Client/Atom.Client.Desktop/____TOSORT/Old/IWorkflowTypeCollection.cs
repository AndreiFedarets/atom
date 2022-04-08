using System.Collections.Generic;

namespace Atom
{
    public interface IWorkflowTypeCollection : IEnumerable<IWorkflowType>
    {
        IWorkflowType Create();
    }
}
