using Atom.Design.Hosting;
using Atom.Design.Reflection;
using Atom.Design.Services;
using Layex.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;

namespace Atom.Client.ViewModels
{
    public sealed class ActionExplorerViewModel : DialogViewModel
    {
        private readonly ICollectionView _collectionView;
        private readonly List<IAction> _actions;
        private IAction _selectedAction;
        public string _searchText;

        public ActionExplorerViewModel(IProject project, IObjectExplorer objectExplorer)
        {
            DisplayName = "Atom Action Explorer";
            _actions = objectExplorer.GetAvailableActions(project);
            _collectionView = CollectionViewSource.GetDefaultView(_actions);
        }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                _collectionView.Filter = null;
                if (!string.IsNullOrWhiteSpace(_searchText))
                {
                    _collectionView.Filter = FilterAction;
                    SelectedAction = null;
                }
                NotifyOfPropertyChange(() => SearchText);
            }
        }

        public IEnumerable<IAction> Actions
        {
            get { return _actions; }
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

        public IAction Action { get; private set; }

        public void Submit()
        {
            Action = SelectedAction;
            TryClose(true);
        }

        public void Cancel()
        {
            TryClose();
        }

        private bool FilterAction(object item)
        {
            if (SearchText.Equals(" "))
            {
                return true;
            }
            string searchText = SearchText;
            IAction action = (IAction)item;
            return action.Title.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
