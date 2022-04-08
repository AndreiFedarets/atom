using Atom.Design.Hosting;
using Atom.Design.Reflection.Metadata;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atom.Design.Services
{
    public sealed class TableDesignerSerializer : ObjectDesignerSerializer
    {
        private readonly ITypeService _typeService;

        public TableDesignerSerializer(ITypeService typeService)
        {
            _typeService = typeService;
        }

        protected override bool CanReadDesigner(XDocument document)
        {
            XElement element = document.Root;
            return string.Equals(element.Name.LocalName, Constants.Serialization.Table.Root, StringComparison.Ordinal);
        }

        protected override IObjectDesigner ReadDesigner(XDocument document, IProject project)
        {
            Table table = new Table();
            XElement element = document.Root;
            ReadSimpleTitle(element, table.Title);
            IEnumerable<XElement> tableValueElements = element.Elements(Constants.Serialization.Table.TableValue);
            foreach (XElement tableValueElement in tableValueElements)
            {
                TableValue tableValue = ReadTableValue(tableValueElement);
                table.Add(tableValue);
            }
            return table;
        }

        private TableValue ReadTableValue(XElement element)
        {
            string name = element.ReadAttribute<string>(Constants.Serialization.Table.ValueName);
            TypeReference type = ReadTypeReference(element);
            TableValue variable = new TableValue(name, type);
            variable.Value = ReadValue(variable.ValueType, element);
            return variable;
        }

        protected override void WriteDesigner(XDocument document, IObjectDesigner designer)
        {
            Table table = (Table)designer;
            XElement element = new XElement(Constants.Serialization.Table.Root);
            WriteSimpleTitle(element, table.Title);
            foreach (TableValue valueDesigner in table)
            {
                WriteTableValue(element, valueDesigner);
            }
            document.Add(element);
        }

        private void WriteTableValue(XElement parentElement, TableValue variable)
        {
            XElement tableValueElement = new XElement(Constants.Serialization.Table.TableValue);
            tableValueElement.WriteAttribute<string>(Constants.Serialization.Table.ValueName, variable.ValueName);
            WriteTypeReference(tableValueElement, variable.ValueType);
            WriteValue(variable.ValueType, variable.Value, tableValueElement);
            parentElement.Add(tableValueElement);
        }

        private object ReadValue(TypeReference type, XElement element)
        {
            XElement valueElement = element.Element(Constants.Serialization.Table.Value);
            return _typeService.ReadValue(type, valueElement);
        }

        private void WriteValue(TypeReference type, object value, XElement element)
        {
            XElement valueElement = new XElement(Constants.Serialization.Table.Value);
            _typeService.WriteValue(type, value, valueElement);
            element.Add(valueElement);
        }
    }
}
