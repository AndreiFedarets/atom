using System;

namespace Atom
{
    public interface IWorkflowExecutor : IDisposable
    {
        IActionInstance CurrentAction { get; }

        bool ExecuteNextAction();
    }
}
