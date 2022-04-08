using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace Atom.Design.ObjectModel.DataTable.Generic
{
    [Guid("7FA50418-2472-44A1-A8FD-1EB5EEBEDF05")]
    internal sealed class DataTableFactory : IDataTableFactory
    {
        public IDataTable Create()
        {
            Guid token = Guid.NewGuid();
            return new DataTable(token);
        }

        public IDataTable Parse(string content)
        {
            XElement tableElement = XElement.Parse(content);
            //Table Properties
            Guid token = tableElement.ReadAttribute<Guid>(Constants.Serialization.Token);
            DataTable table = new DataTable(token);
            //Table Layout
            XElement layoutElement = tableElement.Element(Constants.Serialization.Layout);
            foreach (XElement columnElement in layoutElement.Elements(Constants.Serialization.Column))
            {
                string columnName = columnElement.ReadAttribute<string>(Constants.Serialization.Name);
                string typeName = columnElement.ReadAttribute<string>(Constants.Serialization.Type);
                ITypeAdapter typeAdapter = TypeAdapterFactory.GetAdapter(typeName);
                table.RowLayout.Add(columnName, typeAdapter);
            }
            //Table Data
            foreach (XElement rowElement in tableElement.Elements(Constants.Serialization.Row))
            {
                DataRow row = table.AddRow();
                XElement[] cellElements = rowElement.Elements(Constants.Serialization.Cell).ToArray();
                for (int i = 0; i < cellElements.Length; i++)
                {
                    XElement cellElement = cellElements[i];
                    string cellValue = cellElement.Value;
                    DataCell cell = row[i];
                    cell.Value = cellValue;
                }
            }
            return table;
        }

        public void Save(string fileFullName, IDataTable dataTable)
        {
            DataTable table = (DataTable)dataTable;
            XElement tableElement = new XElement(Constants.Serialization.Table);
            //Table Properties
            tableElement.WriteAttribute<Guid>(Constants.Serialization.Token, table.Token);
            tableElement.WriteAttribute<Guid>(Constants.Serialization.FactoryToken, GetType().GUID);
            //Table Layout
            XElement layoutElement = new XElement(Constants.Serialization.Layout);
            foreach (ColumnLayout column in table.RowLayout)
            {
                XElement columnElement = new XElement(Constants.Serialization.Column);
                columnElement.WriteAttribute<string>(Constants.Serialization.Name, column.Name);
                columnElement.WriteAttribute<string>(Constants.Serialization.Type, column.Type.TypeName);
                layoutElement.Add(columnElement);
            }
            tableElement.Add(layoutElement);
            //Table Data
            foreach (DataRow row in table)
            {
                XElement rowElement = new XElement(Constants.Serialization.Row);
                foreach (DataCell cell in row)
                {
                    rowElement.WriteElement<string>(Constants.Serialization.Cell, cell.Value);
                }
                tableElement.Add(rowElement);
            }
            tableElement.Save(fileFullName);
        }
    }
}
