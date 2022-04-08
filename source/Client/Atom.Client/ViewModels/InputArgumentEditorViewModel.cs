using Atom.Design;
using Atom.Design.Reflection;
using Atom.Design.Services;
using Layex.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Atom.Client.ViewModels
{
    public sealed class InputArgumentEditorViewModel : ViewModel
    {
        private readonly InputArgument _inputArgument;
        private readonly List<ValueSourceSelectorViewModel> _availableValueSelectors;
        private ValueSourceSelectorViewModel _selectedValueSelector;

        public InputArgumentEditorViewModel(InputArgument inputArgument, IObjectExplorer objectExplorer)
        {
            DisplayName = "Edit Input Argument";
            _inputArgument = inputArgument;
            Method method = DesignerHelpers.GetParent<Method>(inputArgument);
            _availableValueSelectors = new List<ValueSourceSelectorViewModel>();
            _availableValueSelectors.Add(new ScopeValueSelectorViewModel(this, method, inputArgument));
            _availableValueSelectors.Add(new TableValueSelectorViewModel(this, inputArgument, method.Document.Project, objectExplorer));
            _selectedValueSelector = _availableValueSelectors.FirstOrDefault();
            SelectCurrectValueSource();
        }

        private void SelectCurrectValueSource()
        {
            BaseValue value = _inputArgument.Value;
            if (value == null)
            {
                return;
            }
            if (value is PropertyValue)
            {
                SelectedValueSelector = _availableValueSelectors.First(x => x is TableValueSelectorViewModel);
            }
            else
            {
                SelectedValueSelector = _availableValueSelectors.First(x => x is ScopeValueSelectorViewModel);
            }
            SelectedValueSelector.SelectCurrentValueSource();
        }

        public IEnumerable<ValueSourceSelectorViewModel> AvailableValueSelectors
        {
            get { return _availableValueSelectors; }
        }

        public ValueSourceSelectorViewModel SelectedValueSelector
        {
            get { return _selectedValueSelector; }
            set
            {
                _selectedValueSelector = value;
                NotifyOfPropertyChange(() => SelectedValueSelector);
            }
        }

        public void Submit()
        {
            BaseValue value = SelectedValueSelector?.SelectedValue;
            if (value != null)
            {
                _inputArgument.Value = value;
                TryClose();
            }
        }

        public void Cancel()
        {
            TryClose();
        }

        private void OnValueSourceSelectorPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ValueSourceSelectorViewModel viewModel = (ValueSourceSelectorViewModel)sender;
            if (viewModel.IsSelected)
            {
                SelectedValueSelector = viewModel;
            }
        }
    }
}
