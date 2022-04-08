using Atom.Design.Hosting;
using Atom.Design.Reflection.Metadata;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Text;

namespace Atom.Design.Services
{
    public abstract class ObjectDesignerCodeGenerator : IDesignerCodeGenerator
    {
        protected readonly ITypeService TypeService;

        public ObjectDesignerCodeGenerator(ITypeService typeService)
        {
            TypeService = typeService;
        }

        public string Generate(IObjectDesigner designer)
        {
            StringBuilder stringBuilder = new StringBuilder();
            CodeCompileUnit compileUnit = GenerateCodeUnit(designer);
            using (StringWriter writer = new StringWriter(stringBuilder))
            {
                using (IndentedTextWriter indentedTextWriter = new IndentedTextWriter(writer, "    "))
                {
                    CodeDomProvider provider = CreateCodeDomProvider(designer.Document.Project.Language);
                    provider.GenerateCodeFromCompileUnit(compileUnit, indentedTextWriter, new CodeGeneratorOptions());
                }
            }
            return stringBuilder.ToString();
        }

        private CodeDomProvider CreateCodeDomProvider(CodeLanguage language)
        {
            switch (language)
            {
                case CodeLanguage.CSharp:
                    return new Microsoft.CSharp.CSharpCodeProvider();
                case CodeLanguage.VisualBasic:
                    return new Microsoft.VisualBasic.VBCodeProvider();
                //case CodeLanguage.JScript:
                //    return new Microsoft.JScript.JScriptCodeProvider();
                default:
                    throw new NotSupportedException();
            }
        }

        private CodeCompileUnit GenerateCodeUnit(IObjectDesigner designer)
        {
            CodeCompileUnit codeUnit = new CodeCompileUnit();
            TypeReference typeReference = designer.GetTypeReference();
            //Namespace
            CodeNamespace @namespace = new CodeNamespace(typeReference.Namespace);
            codeUnit.Namespaces.Add(@namespace);
            //Type
            CodeTypeDeclaration type = CreateType(typeReference, designer);
            @namespace.Types.Add(type);
            //Members
            GenerateMembers(type, typeReference, designer);
            return codeUnit;
        }

        protected virtual CodeTypeDeclaration CreateType(TypeReference typeReference, IObjectDesigner designer)
        {
            CodeTypeDeclaration type = new CodeTypeDeclaration(typeReference.Name);
            type.TypeAttributes = TypeAttributes.Public;
            return type;
        }

        protected abstract void GenerateMembers(CodeTypeDeclaration type, TypeReference typeReference, IObjectDesigner designer);

        protected virtual CodeMemberProperty CreateProperty(string propertyName, TypeReference returnType)
        {
            CodeMemberProperty property = new CodeMemberProperty();
            property.Attributes = MemberAttributes.Public | MemberAttributes.Static;
            property.Type = new CodeTypeReference(returnType.FullName);
            property.Name = propertyName;
            return property;
        }

        protected virtual CodeMemberMethod CreateMethod(string methodName, TypeReference returnType, bool @static = true)
        {
            CodeMemberMethod method = new CodeMemberMethod();
            method.Attributes = MemberAttributes.Public;
            if (@static)
            {
                method.Attributes |= MemberAttributes.Static;
            }
            method.Name = methodName;
            method.ReturnType = returnType == null ? new CodeTypeReference(typeof(void)) : new CodeTypeReference(returnType.FullName);
            return method;
        }

        protected virtual CodeAttributeDeclaration CreateAttribute(Type attributeType, params object[] attributeArguments)
        {
            CodeAttributeArgument[] arguments = new CodeAttributeArgument[attributeArguments.Length];
            for (int i = 0; i < arguments.Length; i++)
            {
                object attributeArgument = attributeArguments[i];
                CodeExpression argumentExpression = new CodePrimitiveExpression(attributeArgument);
                arguments[i] = new CodeAttributeArgument(argumentExpression);
            }
            CodeAttributeDeclaration attribute = new CodeAttributeDeclaration(attributeType.FullName, arguments);
            return attribute;
        }
    }
}
