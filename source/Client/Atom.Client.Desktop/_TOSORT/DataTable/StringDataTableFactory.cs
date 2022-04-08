using System;
using System.CodeDom;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace Atom.Design.ObjectModel.DataTable
{
    [Guid("7FA50418-2472-44A1-A8FD-1EB5EEBEDF05")]
    internal sealed class StringDataTableFactory : BaseDataTableFactory<StringDataTable, string>
    {
        public override string DisplayName
        {
            get { return "String Data Table"; }
        }

        protected override void FillPropertyMember(CodeMemberProperty property, DataTableItem<string> member)
        {
            property.Type = new CodeTypeReference(typeof(string));
            CodePrimitiveExpression primitiveExpression = new CodePrimitiveExpression(member.Value);
            CodeMethodReturnStatement returnStatement = new CodeMethodReturnStatement(primitiveExpression);
            property.GetStatements.Add(returnStatement);
        }

        protected override string ParseValue(XElement element)
        {
            return element.Value;
        }

        protected override void SerializeValue(XElement element, string value)
        {
            element.Value = value;
        }
    }
}
