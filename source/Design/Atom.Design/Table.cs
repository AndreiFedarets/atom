using Atom.Design.Hosting;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Collections;

namespace Atom.Design
{
    public sealed class Table : ItemsControl, IObjectDesigner, IEnumerable<TableValue>
    {
        static Table()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Table), new FrameworkPropertyMetadata(typeof(Table)));
        }

        public Table()
        {
            Title = new SimpleTitle();
        }

        public SimpleTitle Title { get; private set; }

        public IDocument Document { get; set; }

        public void Add(TableValue value)
        {
            if (!Items.Contains(value))
            {
                Items.Add(value);
                DesignerEvents.RaiseDesignerChanged(this);
            }
        }

        public void Remove(TableValue value)
        {
            if (Items.Contains(value))
            {
                Items.Remove(value);
                DesignerEvents.RaiseDesignerChanged(this);
            }
        }

        public IEnumerator<TableValue> GetEnumerator()
        {
            return Items.OfType<TableValue>().GetEnumerator();
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
