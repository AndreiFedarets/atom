using Atom.Design.Hosting;
using System;
using System.Xml.Linq;

namespace Atom.Design.Services
{
    public sealed class ActionDesignerSerializer : MethodDesignerSerializer
    {
        public ActionDesignerSerializer(IAssemblyManager assemblyManager, ITypeService typeService)
            : base(assemblyManager, typeService)
        {
        }

        protected override bool CanReadDesigner(XDocument document)
        {
            XElement element = document.Root;
            return string.Equals(element.Name.LocalName, Constants.Serialization.Action.Root, StringComparison.Ordinal);
        }

        protected override IObjectDesigner ReadDesigner(XDocument document, IProject context)
        {
            XElement element = document.Element(Constants.Serialization.Action.Root);
            Action action = new Action();
            XElement instructionsElement = element.Element(Constants.Serialization.Method.Instructions);
            ReadInstructionCollection(instructionsElement, action.Instructions, context);
            XElement parametersElement = element.Element(Constants.Serialization.Method.Parameters);
            ReadParameterCollection(parametersElement, action);
            XElement titleElement = element.Element(Constants.Serialization.Method.Title);
            ReadTitle(titleElement, action.Title);
            return action;
        }

        protected override void WriteDesigner(XDocument document, IObjectDesigner designer)
        {
            Action action = (Action)designer;
            XElement element = new XElement(Constants.Serialization.Action.Root);
            WriteTitle(element, action.Title);
            WriteParameterCollection(element, action);
            WriteInstructionCollection(element, action);
            document.Add(element);
        }
    }
}
