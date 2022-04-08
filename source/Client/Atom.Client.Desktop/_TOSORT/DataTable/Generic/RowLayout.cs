using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Atom.Design.ObjectModel.DataTable.Generic
{
    public sealed class RowLayout : ReadOnlyCollection<ColumnLayout>
    {
        public RowLayout()
            : base(new List<ColumnLayout>())
        {
        }

        public event EventHandler<ColumnLayoutEventArgs> ColumnAdded;

        public event EventHandler<ColumnLayoutEventArgs> ColumnRemoved;

        public void Add(string name, ITypeAdapter typeAdapter)
        {
            ColumnLayout column = new ColumnLayout(name, typeAdapter);
            Items.Add(column);
            ColumnLayoutEventArgs.RaiseEvent(this, ColumnAdded, column);
        }

        public void Remove(string name)
        {
            ColumnLayout column = Items.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.Ordinal));
            RemoveInternal(column);
        }

        public void Clear()
        {
            foreach (ColumnLayout column in Items)
            {
                RemoveInternal(column);
            }
        }

        private void RemoveInternal(ColumnLayout column)
        {
            if (column != null)
            {
                Items.Remove(column);
                ColumnLayoutEventArgs.RaiseEvent(this, ColumnRemoved, column);
            }
        }
    }
}
