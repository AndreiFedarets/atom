using Atom.Design;
using Atom.Runtime;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Atom.Client.Win.ViewModels
{
    public class ActionsExplorerViewModel : ViewModel
    {
        private readonly IApplication _application;
        private readonly ObservableCollection<IAction> _actions;
        private IAction _selectedAction;

        public ActionsExplorerViewModel(IApplication application)
        {
            _application = application;
            _actions = new ObservableCollection<IAction>();
            Reload();
        }

        public override string DisplayName
        {
            get { return "Actions Store"; }
            set { }
        }

        public IAssemblyCollection Assemblies
        {
             get { return _application.Assemblies; }
        }


        public IAction SelectedAction
        {
            get { return _selectedAction; }
            set
            {
                _selectedAction = value;
                NotifyOfPropertyChange(() => SelectedAction);
            }
        }

        public IEnumerable<IAction> Actions
        {
            get { return _actions; }
        }

        public void Reload()
        {
            //if (_activityStorage != null)
            //{
            //    _activityStorage.Added -= OnAdded;
            //    _activityStorage.Removed -= OnRemoved;
            //}
            //Activities.Clear();
            //if (Project != null)
            //{
            //    _activityStorage = Project.Resolve<IActivityStorage>();
            //    _activityStorage.Added += OnAdded;
            //    _activityStorage.Removed += OnRemoved;
            //    foreach (IActivity activity in _activityStorage)
            //    {
            //        Activities.Add(activity);
            //    }
            //}
            //ForceNotify();
            foreach (IAssembly actionAssembly in _application.Assemblies)
            {
                foreach (IActionType actionType in actionAssembly.EnumerateActions())
                {
                    _actions.Add(actionType);
                }
            }
        }

        public void SelectAction()
        {
            TryClose(true);
        }
    }
}
