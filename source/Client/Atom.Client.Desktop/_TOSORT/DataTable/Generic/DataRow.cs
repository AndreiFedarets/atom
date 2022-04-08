using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Atom.Design.ObjectModel.DataTable.Generic
{
    public sealed class DataRow : ReadOnlyCollection<DataCell>
    {
        private readonly RowLayout _rowLayout;

        public DataRow(RowLayout rowLayout)
            : base(new List<DataCell>())
        {
            _rowLayout = rowLayout;
            foreach (ColumnLayout cellLayout in _rowLayout)
            {
                DataCell dataCell = new DataCell(cellLayout);
                Items.Add(dataCell);
            }
            _rowLayout.ColumnAdded += OnColumnAdded;
            _rowLayout.ColumnRemoved += OnColumnRemoved;
        }

        public string Name { get; set; }

        private void OnColumnAdded(object sender, ColumnLayoutEventArgs e)
        {
            DataCell dataCell = new DataCell(e.Column);
            Items.Add(dataCell);
        }

        private void OnColumnRemoved(object sender, ColumnLayoutEventArgs e)
        {
            DataCell dataCell = Items.FirstOrDefault(x => ReferenceEquals(x.Layout, e.Column));
            Items.Remove(dataCell);
        }
    }
}
