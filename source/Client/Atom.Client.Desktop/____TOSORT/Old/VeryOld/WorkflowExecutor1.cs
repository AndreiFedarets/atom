using System.Collections.Generic;
using Atom.Execution;

namespace Atom
{
    internal sealed class WorkflowExecutor : IWorkflowExecutor
    {
        private readonly object _lock;
        private readonly IWorkflow _workflow;
        private IEnumerator<IActionInstance> _enumerator;
        private IScope _scope;

        internal WorkflowExecutor(IWorkflow workflow, IScope scope)
        {
            _lock = new object();
            _workflow = workflow;
            _scope = scope;
            _enumerator = _workflow.Actions.GetEnumerator();
            _enumerator.MoveNext();
        }

        public IActionInstance CurrentAction
        {
            get
            {
                lock (_lock)
                {
                    return _enumerator.Current;
                }
            }
        }

        public bool ExecuteAction()
        {
            lock (_lock)
            {
                if (_enumerator == null)
                {
                    _workflow.Lock();
                    _enumerator = _workflow.Actions.GetEnumerator();
                }
                if (CurrentAction != null)
                {
                    _scope = CurrentAction.Execute(_scope);
                    _enumerator.MoveNext();
                }
                else
                {
                    _enumerator = null;
                }
                return CurrentAction != null;
            }
        }

        public void Abort()
        {
            _enumerator = null;
        }
    }
}
