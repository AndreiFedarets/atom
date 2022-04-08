using Atom.Client.Commands;
using Atom.Design.ObjectModel.DataTable.Generic;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Atom.Client.ViewModels.DataTableDesign
{
    public class ManageGenericRowLayoutViewModel : ViewModel
    {
        private readonly ObservableCollection<ManageGenericColumnLayoutViewModel> _columns;
        private readonly RowLayout _rowLayout;

        public ManageGenericRowLayoutViewModel(RowLayout rowLayout)
        {
            _rowLayout = rowLayout;
            _columns = new ObservableCollection<ManageGenericColumnLayoutViewModel>();
            SubmitCommand = new SyncCommand(Submit);
            AddColumnCommand = new SyncCommand(AddColumn);
        }

        public IEnumerable<ManageGenericColumnLayoutViewModel> Columns
        {
            get { return _columns; }
        }

        public ICommand SubmitCommand { get; private set; }

        public ICommand AddColumnCommand { get; private set; }

        private void Submit()
        {
            _rowLayout.Clear();
            foreach (ManageGenericColumnLayoutViewModel childViewModel in Columns)
            {
                _rowLayout.Add(childViewModel.ColumnName, childViewModel.ColumnType);
            }
        }

        public void AddColumn()
        {
            _columns.Add(new ManageGenericColumnLayoutViewModel(this));
        }

        public void RemoveColumn(ManageGenericColumnLayoutViewModel childViewModel)
        {
            _columns.Remove(childViewModel);
        }
    }
}
