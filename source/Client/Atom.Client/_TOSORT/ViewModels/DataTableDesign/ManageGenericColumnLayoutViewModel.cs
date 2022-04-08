using Atom.Client.Commands;
using Atom.Design.ObjectModel.DataTable.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Atom.Client.ViewModels.DataTableDesign
{
    public class ManageGenericColumnLayoutViewModel : ViewModel
    {
        private readonly ManageGenericRowLayoutViewModel _parentViewModel;
        private string _columnName;

        public ManageGenericColumnLayoutViewModel(ManageGenericRowLayoutViewModel parentViewModel)
        {
            _parentViewModel = parentViewModel;
            AvailableTypes = TypeAdapterFactory.AvailableTypes;
            ColumnType = AvailableTypes.First();
            RemoveCommand = new SyncCommand(Remove);
        }

        public string ColumnName
        {
            get { return _columnName; }
            set
            {
                _columnName = value;
                NotifyOfPropertyChange(() => ColumnName);
            }
        }

        public IEnumerable<ITypeAdapter> AvailableTypes { get; private set; }

        public ITypeAdapter ColumnType { get; set; }
        
        public ICommand RemoveCommand { get; private set; }
        
        private void Remove()
        {
            _parentViewModel.RemoveColumn(this);
        }
    }
}
