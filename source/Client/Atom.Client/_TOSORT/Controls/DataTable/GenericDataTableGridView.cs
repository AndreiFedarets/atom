using Atom.Design.ObjectModel.DataTable.Generic;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xml;

namespace Atom.Client.Controls.DataTable
{
    public class GenericDataTableGridView : ListView
    {
        public static readonly DependencyProperty DataTableProperty;

        static GenericDataTableGridView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GenericDataTableGridView), new FrameworkPropertyMetadata(typeof(GenericDataTableGridView)));
            DataTableProperty = DependencyProperty.Register("DataTable", typeof(Design.ObjectModel.DataTable.Generic.DataTable), typeof(GenericDataTableGridView), new FrameworkPropertyMetadata(OnDataTablePropertyChanged));
        }
        public GenericDataTableGridView()
        {
            View = new GridView();
        }

        public Design.ObjectModel.DataTable.Generic.DataTable DataTable
        {
            get { return (Design.ObjectModel.DataTable.Generic.DataTable)GetValue(DataTableProperty); }
            set { SetValue(DataTableProperty, value); }
        }

        private GridView GridView
        {
            get { return (GridView)View; }
        }
        
        private void Initialize()
        {
            if (DataTable == null)
            {
                return;
            }
            foreach (ColumnLayout column in DataTable.RowLayout)
            {
                AddColumnLayout(column);
            }
            ItemsSource = DataTable;
            DataTable.RowLayout.ColumnAdded += OnLayoutColumnAdded;
            DataTable.RowLayout.ColumnRemoved += OnLayoutColumnRemoved;
        }

        private void OnLayoutColumnAdded(object sender, ColumnLayoutEventArgs e)
        {
            AddColumnLayout(e.Column);
        }

        private void OnLayoutColumnRemoved(object sender, ColumnLayoutEventArgs e)
        {
            RemoveColumnLayout(e.Column);
        }

        private void AddColumnLayout(ColumnLayout column)
        {
            GridViewColumn gridViewColumn = new GridViewColumn();
            gridViewColumn.Header = column.Name;
            DataTemplate cellTemplate = CreateDataTemplate(column);
            gridViewColumn.CellTemplate = cellTemplate;
            GridView.Columns.Add(gridViewColumn);
        }

        private void RemoveColumnLayout(ColumnLayout column)
        {
            GridViewColumn gridViewColumn = GridView.Columns.FirstOrDefault(x => string.Equals(x.Header.ToString(), column.Name, StringComparison.Ordinal));
            GridView.Columns.Remove(gridViewColumn);
        }

        private DataTemplate CreateDataTemplate(ColumnLayout column)
        {
            int columnIndex = DataTable.RowLayout.IndexOf(column);
            string dataTemplateText = Properties.Resources.GenericDataTableTextTemplate.Replace("{index}", columnIndex.ToString());
            StringReader stringReader = new StringReader(dataTemplateText);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            DataTemplate dataTemplate = XamlReader.Load(xmlReader) as DataTemplate;
            dataTemplate.DataType = typeof(DataCell);
            return dataTemplate;
        }

        private static void OnDataTablePropertyChanged(object sender, DependencyPropertyChangedEventArgs eventArgs)
        {
            GenericDataTableGridView dataTable = (GenericDataTableGridView)sender;
            dataTable.Initialize();
        }
    }
}
