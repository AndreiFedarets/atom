using System.Collections.Generic;
using System.Windows.Input;
using Atom.Client.Win.Commands;
using Atom.Design;
using Atom.Runtime;

namespace Atom.Client.Win.ViewModels
{
    public class ActionDesignerViewModel : ViewModel
    {
        private readonly IWindowsManager _windowManager;
        private readonly IApplication _application;
        private IWorkflow _workflow;
        private bool _enableArgumentsAutoBinding;

        public ActionDesignerViewModel(IWorkflow workflow, IApplication application, IWindowsManager windowManager)
        {
            _workflow = workflow;
            _application = application;
            _windowManager = windowManager;
            AddActionCommand = new SyncCommand(AddAction);
            RunActionCommand = new SyncCommand(RunAction);
            DebugActionCommand = new SyncCommand(DebugAction);
            _enableArgumentsAutoBinding = true;
        }

        public override string DisplayName
        {
            get { return "Action Designer"; }
            set { }
        }

        public bool EnableArgumentsAutoBinding
        {
            get { return _enableArgumentsAutoBinding; }
            set
            {
                _enableArgumentsAutoBinding = value;
                NotifyOfPropertyChange(() => EnableArgumentsAutoBinding);
            }
        }

        public IEnumerable<IInstance> Actions
        {
            get { return _workflow; }
        }

        public ICommand AddActionCommand { get; private set; }

        public ICommand RunActionCommand { get; private set; }

        public ICommand DebugActionCommand { get; private set; }

        //TODO: bad approach, think about INotifyCollectionChanged in IInstanceCollection
        private void NotifyActionsChanged()
        {
            IWorkflow workflow = _workflow;
            _workflow = null;
            NotifyOfPropertyChange(() => Actions);
            _workflow = workflow;
            NotifyOfPropertyChange(() => Actions);
        }

        private void RunAction()
        {
            _application.Runtime.Execute(_workflow);
        }

        private void DebugAction()
        {
            _application.Runtime.Debug(_workflow);
        }

        private void AddAction()
        {
            IAction action = _windowManager.ShowActionsExplorer();
            if (action != null)
            {
                IInstance actionInstance = _workflow.Add(action);
                if (EnableArgumentsAutoBinding)
                {
                    _application.ArgumentBinder.AutoBindInputArguments(actionInstance, _workflow);
                }
                NotifyActionsChanged();
            }
        }
    }
}
