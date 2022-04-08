using System.Collections.Generic;

namespace Atom
{
    internal interface IWorkflowInstance : IActionInstance, IEnumerable<IActionInstance>
    {
    }
}
