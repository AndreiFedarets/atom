using Atom.Design.Reflection.Metadata;
using Atom.Runtime;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

namespace Atom.Design.Services
{
    public sealed class TableDesignerCodeGenerator : ObjectDesignerCodeGenerator
    {
        public TableDesignerCodeGenerator(ITypeService typeService)
            : base(typeService)
        {
        }

        protected override CodeTypeDeclaration CreateType(TypeReference typeReference, IObjectDesigner designer)
        {
            Table tableDesigner = (Table)designer;
            CodeTypeDeclaration type = base.CreateType(typeReference, designer);
            CodeAttributeDeclaration attribute = CreateAttribute(typeof(DataTableAttribute), tableDesigner.Title.Text);
            type.CustomAttributes.Add(attribute);
            return type;
        }

        protected override void GenerateMembers(CodeTypeDeclaration type, TypeReference typeReference, IObjectDesigner designer)
        {
            Table table = (Table)designer;
            foreach (TableValue tableValue in table)
            {
                CodeMemberProperty property = CreateProperty(tableValue.ValueName, tableValue.ValueType);
                IEnumerable<CodeStatement> codeStatements = TypeService.GenerateCode(tableValue.ValueType, tableValue.Value);
                foreach (CodeStatement statement in codeStatements)
                {
                    property.GetStatements.Add(statement);
                }
                type.Members.Add(property);
            }
        }


        //public DataTableDesignerCodeGenerator(IDataItemCodeGenerator dataItemCodeGenerator)
        //{
        //    _dataItemCodeGenerator = dataItemCodeGenerator;
        //}

        //public string Generate(IObjectDesigner designer)
        //{
        //    TableDesigner dataTableDesigner = designer as TableDesigner;
        //    if (dataTableDesigner == null)
        //    {
        //        return string.Empty;
        //    }
        //    StringBuilder stringBuilder = new StringBuilder();
        //    CodeCompileUnit compileUnit = GenerateDataTable(dataTableDesigner);
        //    using (StringWriter writer = new StringWriter(stringBuilder))
        //    {
        //        using (IndentedTextWriter indentedTextWriter = new IndentedTextWriter(writer, "    "))
        //        {
        //            CSharpCodeProvider provider = new CSharpCodeProvider();
        //            provider.GenerateCodeFromCompileUnit(compileUnit, indentedTextWriter, new CodeGeneratorOptions());
        //        }
        //    }
        //    return stringBuilder.ToString();
        //}

        //private CodeCompileUnit GenerateDataTable(TableDesigner dataTable)
        //{
        //    TypeReference dataTableType = dataTable.Type;
        //    CodeCompileUnit compileUnit = new CodeCompileUnit();
        //    //Namespace
        //    CodeNamespace @namespace = new CodeNamespace(dataTableType.Namespace);
        //    compileUnit.Namespaces.Add(@namespace);
        //    //Class
        //    CodeTypeDeclaration type = new CodeTypeDeclaration(dataTableType.TypeName);
        //    type.TypeAttributes = TypeAttributes.Public; // Static?
        //    CodeTypeReference attributeTypeReference = new CodeTypeReference(typeof(DataTableAttribute));
        //    CodeAttributeArgument designTokenAttributeArgument = new CodeAttributeArgument(new CodePrimitiveExpression(GetType().GUID.ToString()));
        //    CodeAttributeDeclaration attributeDeclaration = new CodeAttributeDeclaration(attributeTypeReference, designTokenAttributeArgument);
        //    type.CustomAttributes.Add(attributeDeclaration);
        //    @namespace.Types.Add(type);
        //    //Properties
        //    foreach (DataItem dataItem in dataTable)
        //    {
        //        CodeMemberProperty property = new CodeMemberProperty();
        //        property.Attributes = MemberAttributes.Public | MemberAttributes.Static;
        //        property.Type = new CodeTypeReference(dataItem.ValueType.FullName);
        //        property.Name = dataItem.Name;
        //        IEnumerable<CodeStatement> dataItemCodeStatements = _dataItemCodeGenerator.GenerateCode(dataItem);
        //        foreach (CodeStatement statement in dataItemCodeStatements)
        //        {
        //            property.GetStatements.Add(statement);
        //        }
        //        type.Members.Add(property);
        //    }
        //    return compileUnit;
        //}
    }
}
