using Atom.Design.Reflection;
using Layex.ViewModels;
using System.Windows.Input;

namespace Atom.Client.ViewModels
{
    public abstract class ValueSourceSelectorViewModel : ViewModel
    {
        private readonly InputArgumentEditorViewModel _parentViewModel;
        private BaseValue _selectedValue;

        public ValueSourceSelectorViewModel(InputArgumentEditorViewModel parentViewModel)
        {
            _parentViewModel = parentViewModel;
        }

        public ICommand SubmitCommand { get; private set; }

        public BaseValue SelectedValue
        {
            get { return _selectedValue; }
            set
            {
                _selectedValue = value;
                NotifyOfPropertyChange(() => SelectedValue);
            }
        }

        public bool IsSelected
        {
            get { return _parentViewModel.SelectedValueSelector == this; }
            set
            {
                if (value)
                {
                    _parentViewModel.SelectedValueSelector = this;
                }
                NotifyOfPropertyChange(() => IsSelected);
            }
        }

        public void Submit()
        {
            if (SelectedValue != null)
            {
                _parentViewModel.Submit();
            }
        }

        internal abstract void SelectCurrentValueSource();
    }
}
