using System;

namespace Atom
{
    public interface IWorkflowType : IActionType, IReadOnlyCollection<IActionInstance>
    {
        IActionInstance Insert(int index, IActionType actionSource);

        IActionInstance Add(IActionType actionSource);

        void Remove(Guid actionInstanceUid);

        new WorkflowTypeMetadata GetMetadata();
    }
}
