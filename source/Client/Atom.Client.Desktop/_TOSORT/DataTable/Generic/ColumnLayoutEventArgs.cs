using System;

namespace Atom.Design.ObjectModel.DataTable.Generic
{
    public sealed class ColumnLayoutEventArgs : EventArgs
    {
        public ColumnLayoutEventArgs(ColumnLayout column)
        {
            Column = column;
        }

        public ColumnLayout Column { get; private set; }

        internal static void RaiseEvent(object sender, EventHandler<ColumnLayoutEventArgs> handler, ColumnLayout column)
        {
            handler?.Invoke(sender, new ColumnLayoutEventArgs(column));
        }
    }
}
