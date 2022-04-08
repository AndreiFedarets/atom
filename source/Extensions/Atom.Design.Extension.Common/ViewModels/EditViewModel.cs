using Layex.ViewModels;

namespace Atom.Design.Extension.Common.ViewModels
{
    public class EditViewModel : ViewModel
    {
        private readonly Table _table;
        private string _valueName;
        private string _value;

        public EditViewModel(Table table)
        {
            _table = table;
        }

        public string ValueName
        {
            get { return _valueName; }
            set
            {
                _valueName = value;
                NotifyOfPropertyChange(() => ValueName);
            }
        }

        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                NotifyOfPropertyChange(() => Value);
            }
        }

        public void Submit()
        {
            TableValue tableValue = new TableValue(_valueName, StringTypeAdapter.TypeReference, _value);
            _table.Add(tableValue);
            TryClose(true);
        }

        public void Cancel()
        {
            TryClose(false);
        }
    }
}
