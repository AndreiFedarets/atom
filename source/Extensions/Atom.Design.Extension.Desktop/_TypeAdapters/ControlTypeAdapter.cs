using Atom.Design.Reflection.Metadata;
using Atom.Design.Reflection.Metadata.Binary;
using Atom.Design.Services;
using Atom.Runtime.Extension.Desktop;
using System.CodeDom;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atom.Design.Extension.Desktop
{
    public abstract class ControlTypeAdapter<TInterface, TClass> : TypeAdapter 
        where TInterface : IControl where TClass : Control
    {
        public static readonly TypeReference InterfaceTypeReference;
        public static readonly TypeReference ClassTypeReference;

        static ControlTypeAdapter()
        {
            InterfaceTypeReference = MetadataProvider.GetReference(typeof(TInterface));
            ClassTypeReference = MetadataProvider.GetReference(typeof(TClass));
        }

        public ControlTypeAdapter()
            : base(InterfaceTypeReference)
        {
        }

        public override IEnumerable<CodeStatement> GenerateCode(object value)
        {
            Control control = (Control)value;
            List<CodeExpression> arguments = GenerateControlArguments((TClass)control);
            yield return new CodeMethodReturnStatement(new CodeObjectCreateExpression(typeof(TClass), arguments.ToArray()));
        }

        protected virtual List<CodeExpression> GenerateControlArguments(TClass control)
        {
            List<CodeExpression> arguments = new List<CodeExpression>();
            foreach (ControlProperty property in control.Properties)
            {
                arguments.Add(new CodeObjectCreateExpression(typeof(ControlProperty),
                    new CodePrimitiveExpression(property.Id),
                    new CodePrimitiveExpression(property.Value)
                ));
            }
            return arguments;
        }
        
        public override object ReadValue(XElement valueElement)
        {
            List<object> arguments = ReadControlArguments(valueElement);
            return System.Activator.CreateInstance(typeof(TClass), arguments.ToArray());
        }

        protected virtual List<object> ReadControlArguments(XElement valueElement)
        {
            List<object> arguments = new List<object>();
            foreach (XElement propertyElement in valueElement.Elements(Constants.Serialization.Property))
            {
                XAttribute idAttribute = propertyElement.Attribute(Constants.Serialization.Id);
                int id = int.Parse(idAttribute.Value);
                XAttribute valueAttribute = propertyElement.Attribute(Constants.Serialization.Value);
                string value = valueAttribute.Value;
                ControlProperty property = new ControlProperty(id, value);
                arguments.Add(property);
            }
            return arguments;
        }

        public override void WriteValue(XElement valueElement, object value)
        {
            WriteControlArguments(valueElement, (TClass)value);
        }

        protected virtual void WriteControlArguments(XElement valueElement, TClass control)
        {
            foreach (ControlProperty property in control.Properties)
            {
                XElement propertyElement = new XElement(Constants.Serialization.Property,
                    new XAttribute(Constants.Serialization.Id, property.Id),
                    new XAttribute(Constants.Serialization.Value, property.Value));
                valueElement.Add(propertyElement);
            }
        }
    }
}
