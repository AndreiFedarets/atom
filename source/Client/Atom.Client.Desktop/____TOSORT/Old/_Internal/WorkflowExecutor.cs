using System.Collections.Generic;

namespace Atom
{
    internal sealed class WorkflowExecutor : IWorkflowExecutor
    {
        private readonly object _lock;
        private readonly IActionInstance _action;
        private IEnumerator<IActionInstance> _enumerator;
        private IScope _scope;
        private ILockable _lockable;

        internal WorkflowExecutor(IActionInstance action, IScope scope)
        {
            _lock = new object();
            _scope = scope;
            _action = action;
            LockAction();
            //_enumerator = _action.GetEnumerator();
            if (_enumerator != null)
            {
                _enumerator.MoveNext();
            }
        }

        private void LockAction()
        {
            _lockable = _action as ILockable;
            if (_lockable != null)
            {
                _lockable.Lock();
            }
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

        public bool ExecuteNextAction()
        {
            lock (_lock)
            {
                if (CurrentAction != null)
                {
                    _scope = ExecuteAction(CurrentAction, _scope);
                    _enumerator.MoveNext();
                }
                return CurrentAction != null;
            }
        }

        private IScope ExecuteAction(IActionInstance action, IScope scope)
        {
            //    //TODO: move this logic to WorkflowExecutor, here should be 'object[] arguments'
            //    object[] arguments = Arguments.PullInputArguments(scope);
            //    IInvokable invokable = (IInvokable)ActionType;
            //    invokable.Invoke(arguments);
            //    Arguments.PushOutputArguments(arguments);
            //    scope = new Scope(scope);
            //    scope.RegisterDataSource(this);
            //    return scope;
            throw Fail.NotImplemented();
        }

        public void Dispose()
        {
            _enumerator = null;
            if (_lockable != null)
            {
                _lockable.Unlock();
            }
        }
    }
}
