using Atom.Design.Hosting;
using System;
using System.Xml.Linq;

namespace Atom.Design.Services
{
    public sealed class WorkflowDesignerSerializer : MethodDesignerSerializer
    {
        public WorkflowDesignerSerializer(IAssemblyManager assemblyManager, ITypeService typeService)
            : base(assemblyManager, typeService)
        {
        }

        protected override bool CanReadDesigner(XDocument document)
        {
            XElement element = document.Root;
            return string.Equals(element.Name.LocalName, Constants.Serialization.Workflow.Root, StringComparison.Ordinal);
        }

        protected override IObjectDesigner ReadDesigner(XDocument document, IProject context)
        {
            XElement element = document.Element(Constants.Serialization.Workflow.Root);
            Workflow workflow = new Workflow();
            XElement instructionsElement = element.Element(Constants.Serialization.Method.Instructions);
            ReadInstructionCollection(instructionsElement, workflow.Instructions, context);
            ReadSimpleTitle(element, workflow.Title);
            return workflow;
        }

        protected override void WriteDesigner(XDocument document, IObjectDesigner designer)
        {
            Workflow workflow = (Workflow)designer;
            XElement element = new XElement(Constants.Serialization.Workflow.Root);
            WriteSimpleTitle(element, workflow.Title);
            WriteInstructionCollection(element, workflow);
            document.Add(element);
        }
    }
}
