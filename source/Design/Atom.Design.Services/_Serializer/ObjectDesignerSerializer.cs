using Atom.Design.Hosting;
using Atom.Design.Reflection.Metadata;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Atom.Design.Services
{
    public abstract class ObjectDesignerSerializer : IDesignerSerializer
    {
        public IObjectDesigner Read(IDocument document)
        {
            XDocument element = XDocument.Load(document.FullName);
            IObjectDesigner designer = ReadDesigner(element, document.Project);
            return designer;
        }

        public bool Write(IObjectDesigner designer)
        {
            File.WriteAllText(designer.Document.FullName, string.Empty);
            XDocument document = new XDocument();
            WriteDesigner(document, designer);
            document.Save(designer.Document.FullName);
            return true;
        }

        public bool CanRead(IDocument document)
        {
            try
            {
                XDocument element = XDocument.Load(document.FullName);
                return CanReadDesigner(element);
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        protected abstract bool CanReadDesigner(XDocument document);

        protected abstract IObjectDesigner ReadDesigner(XDocument document, IProject context);

        protected abstract void WriteDesigner(XDocument document, IObjectDesigner designer);

        protected MethodReference ReadMethodReference(XElement parentElement)
        {
            XElement element = parentElement.Element(Constants.Serialization.Reflection.Method);
            string name = element.ReadAttribute<string>(Constants.Serialization.Reflection.Name);
            TypeReference typeReference = ReadTypeReference(element);
            ParameterReferenceCollection parameterReferences = ReadParameterReferences(element);
            MethodReference methodReference = new MethodReference(name, parameterReferences, typeReference);
            return methodReference;
        }

        protected ParameterReferenceCollection ReadParameterReferences(XElement parentElement)
        {
            List<ParameterReference> collection = new List<ParameterReference>();
            XElement element = parentElement.Element(Constants.Serialization.Reflection.Parameters);
            foreach (XElement parameterElement in element.Elements(Constants.Serialization.Reflection.Parameter))
            {
                string parameterName = parameterElement.ReadAttribute<string>(Constants.Serialization.Reflection.Name);
                ParameterDirection parameterDirection = parameterElement.ReadAttribute<ParameterDirection>(Constants.Serialization.Reflection.ParameterDirection);
                TypeReference parameterType = ReadTypeReference(parameterElement);
                ParameterReference parameterReference = new ParameterReference(parameterName, parameterDirection, parameterType);
                collection.Add(parameterReference);
            }
            ParameterReferenceCollection parameterReferences = new ParameterReferenceCollection(collection);
            return parameterReferences;
        }

        protected TypeReference ReadTypeReference(XElement parentElement)
        {
            return ReadTypeReference(parentElement, Constants.Serialization.Reflection.Type);
        }

        protected TypeReference ReadTypeReference(XElement parentElement, string elementName)
        {
            XElement element = parentElement.Element(elementName);
            string name = element.ReadAttribute<string>(Constants.Serialization.Reflection.Name);
            string @namespace = element.ReadAttribute<string>(Constants.Serialization.Reflection.Namespace);
            AssemblyReference assemblyReference = ReadAssemblyReference(element);
            TypeReference typeReference = new TypeReference(name, @namespace, assemblyReference);
            return typeReference;
        }

        protected PropertyReference ReadPropertyReference(XElement parentElement)
        {
            XElement element = parentElement.Element(Constants.Serialization.Reflection.Property);
            string name = element.ReadAttribute<string>(Constants.Serialization.Reflection.Name);
            TypeReference declaringType = ReadTypeReference(element, Constants.Serialization.Reflection.DeclaringType);
            TypeReference propertyType = ReadTypeReference(element, Constants.Serialization.Reflection.PropertyType);
            PropertyReference propertyReference = new PropertyReference(name, propertyType, declaringType);
            return propertyReference;
        }

        protected AssemblyReference ReadAssemblyReference(XElement parentElement)
        {
            XElement element = parentElement.Element(Constants.Serialization.Reflection.Assembly);
            string name = element.ReadAttribute<string>(Constants.Serialization.Reflection.Name);
            AssemblyReference assemblyReference = new AssemblyReference(name);
            return assemblyReference;
        }

        protected void ReadSimpleTitle(XElement parentElement, SimpleTitle simpleTitle)
        {
            XElement simpleTitleElement = parentElement.Element(Constants.Serialization.Method.Title);
            if (simpleTitleElement != null)
            {
                simpleTitle.Text = simpleTitleElement.Value;
            }
        }

        protected void ReadTitleText(XElement parentElement, TitleText titleText)
        {
            XElement titleTextElement = parentElement.Element(Constants.Serialization.Method.TitleText);
            if (titleTextElement != null)
            {
                titleText.Text = titleTextElement.Value;
            }
        }

        protected void WriteMethodReference(XElement parentElement, MethodReference methodReference)
        {
            XElement element = new XElement(Constants.Serialization.Reflection.Method);
            element.WriteAttribute<string>(Constants.Serialization.Reflection.Name, methodReference.Name);
            WriteTypeReference(element, methodReference.DeclaringType);
            WriteParameterReferences(element, methodReference.Parameters);
            parentElement.Add(element);
        }

        protected void WriteParameterReferences(XElement parentElement, ParameterReferenceCollection parameterReferences)
        {
            XElement element = new XElement(Constants.Serialization.Reflection.Parameters);
            foreach (ParameterReference parameterReference in parameterReferences)
            {
                XElement parameterElement = new XElement(Constants.Serialization.Reflection.Parameter);
                parameterElement.WriteAttribute<string>(Constants.Serialization.Reflection.Name, parameterReference.Name);
                parameterElement.WriteAttribute<ParameterDirection>(Constants.Serialization.Reflection.ParameterDirection, parameterReference.Direction);
                WriteTypeReference(parameterElement, parameterReference.ParameterType);
                element.Add(parameterElement);
            }
            parentElement.Add(element);
        }

        protected void WriteTypeReference(XElement parentElement, TypeReference typeReference)
        {
            WriteTypeReference(parentElement, typeReference, Constants.Serialization.Reflection.Type);
        }

        protected void WriteTypeReference(XElement parentElement, TypeReference typeReference, string elementName)
        {
            XElement element = new XElement(elementName);
            element.WriteAttribute<string>(Constants.Serialization.Reflection.Name, typeReference.Name);
            element.WriteAttribute<string>(Constants.Serialization.Reflection.Namespace, typeReference.Namespace);
            WriteAssemblyReference(element, typeReference.Assembly);
            parentElement.Add(element);
        }

        protected void WritePropertyReference(XElement parentElement, PropertyReference propertyReference)
        {
            XElement element = new XElement(Constants.Serialization.Reflection.Property);
            element.WriteAttribute<string>(Constants.Serialization.Reflection.Name, propertyReference.Name);
            WriteTypeReference(element, propertyReference.DeclaringType, Constants.Serialization.Reflection.DeclaringType);
            WriteTypeReference(element, propertyReference.PropertyType, Constants.Serialization.Reflection.PropertyType);
            parentElement.Add(element);
        }

        protected void WriteAssemblyReference(XElement parentElement, AssemblyReference assemblyReference)
        {
            XElement element = new XElement(Constants.Serialization.Reflection.Assembly);
            element.WriteAttribute<string>(Constants.Serialization.Reflection.Name, assemblyReference.Name);
            parentElement.Add(element);
        }

        protected void WriteSimpleTitle(XElement parentElement, SimpleTitle simpleTitle)
        {
            XElement simpleTitleElement = new XElement(Constants.Serialization.Method.Title);
            simpleTitleElement.Value = simpleTitle.Text;
            parentElement.Add(simpleTitleElement);
        }

        protected void WriteTitleText(XElement parentElement, TitleText titleText)
        {
            XElement titleTextElement = new XElement(Constants.Serialization.Method.TitleText);
            titleTextElement.Value = titleText.Text;
            parentElement.Add(titleTextElement);
        }
    }
}
