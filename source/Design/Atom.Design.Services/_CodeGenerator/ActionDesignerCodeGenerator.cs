using Atom.Design.Reflection.Metadata;
using Atom.Runtime;
using System.CodeDom;

namespace Atom.Design.Services
{
    public sealed class ActionDesignerCodeGenerator : MethodDesignerCodeGenerator
    {
        public ActionDesignerCodeGenerator(ITypeService typeService)
            : base(typeService)
        {
        }

        protected override void GenerateMembers(CodeTypeDeclaration type, TypeReference typeReference, IObjectDesigner designer)
        {
            Action action = (Action)designer;
            string methodName = action.GetMethodName();
            CodeMemberMethod method = CreateMethod(methodName, null);
            GenerateParameters(method, action.Parameters);
            GenerateMethodBody(method, action);
            GenerateAttributes(method, action);
            type.Members.Add(method);
        }

        private void GenerateAttributes(CodeMemberMethod method, Action action)
        {
            CodeAttributeDeclaration attribute = CreateAttribute(typeof(ActionMethodAttribute), action.Title.ToString());
            method.CustomAttributes.Add(attribute);
        }
    }
}
