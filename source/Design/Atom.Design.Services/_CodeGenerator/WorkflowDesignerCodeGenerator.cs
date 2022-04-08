using Atom.Design.Hosting;
using Atom.Design.Reflection.Metadata;
using Atom.Runtime;
using System;
using System.CodeDom;
using System.Linq;

namespace Atom.Design.Services
{
    public sealed class WorkflowDesignerCodeGenerator : MethodDesignerCodeGenerator
    {
        private readonly ITestFrameworkService _frameworkService;

        public WorkflowDesignerCodeGenerator(ITypeService typeService, ITestFrameworkService frameworkService)
            : base(typeService)
        {
            _frameworkService = frameworkService;
        }

        protected override void GenerateMembers(CodeTypeDeclaration type, TypeReference typeReference, IObjectDesigner designer)
        {
            Workflow workflow = (Workflow)designer;
            string methodName = workflow.GetMethodName();
            CodeMemberMethod method = CreateMethod(methodName, null, false);
            GenerateMethodBody(method, workflow);
            GenerateAttributes(method, workflow);
            type.Members.Add(method);
            ITestFrameworkAdapter frameworkAdapter = _frameworkService.GetAdapter(designer.Document.Project);
            frameworkAdapter.GenerateAttributes(type, method, workflow);
        }

        private void GenerateAttributes(CodeMemberMethod method, Workflow workflow)
        {
            CodeAttributeDeclaration workflowAttribute = CreateAttribute(typeof(WorkflowMethodAttribute), workflow.Title.ToString());
            method.CustomAttributes.Add(workflowAttribute);
        }
    }
}
