using System;
using System.Collections.ObjectModel;

namespace Atom.Design.ObjectModel.DataTable.Generic
{
    [DataTableDesigner("Atom.Client.Views.DataTableDesign.GenericDataTableView, Atom.Client", "Atom.Client.ViewModels.DataTableDesign.GenericDataTableViewModel, Atom.Client")]
    public sealed class DataTable : ObservableCollection<DataRow>, IDataTable
    {
        public DataTable(Guid token)
        {
            Token = token;
            RowLayout = new RowLayout();
        }

        public Guid Token { get; private set; }

        public RowLayout RowLayout { get; private set; }

        public DataRow AddRow()
        {
            DataRow row = new DataRow(RowLayout);
            Add(row);
            return row;
        }
    }
}
