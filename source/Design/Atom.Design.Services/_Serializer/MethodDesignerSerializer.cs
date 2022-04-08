using Atom.Design.Hosting;
using Atom.Design.Reflection;
using Atom.Design.Reflection.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Atom.Design.Services
{
    public abstract class MethodDesignerSerializer : ObjectDesignerSerializer
    {
        private readonly ITypeService _typeService;
        private readonly IAssemblyManager _assemblyManager;

        public MethodDesignerSerializer(IAssemblyManager assemblyManager, ITypeService typeService)
        {
            _assemblyManager = assemblyManager;
            _typeService = typeService;
        }

        protected void ReadInstructionCollection(XElement element, InstructionCollection instructions, IProject context)
        {
            if (element == null)
            {
                return;
            }
            foreach (XElement instructionElement in element.Elements())
            {
                ReadInstruction(instructionElement, instructions, context);
            }
        }

        protected void ReadInstruction(XElement element, InstructionCollection instructions, IProject context)
        {
            if (string.Equals(element.Name.LocalName, Constants.Serialization.Method.Invoke, StringComparison.Ordinal))
            {
                ReadInvokeInstruction(element, instructions, context);
            }
        }

        protected void ReadInvokeInstruction(XElement element, InstructionCollection instructions, IProject context)
        {
            MethodReference methodReference = ReadMethodReference(element);
            IAssembly assembly = _assemblyManager.GetAssembly(methodReference.DeclaringType.Assembly, context);
            IAction action = assembly.Actions[methodReference];
            InvokeInstruction invoke = new InvokeInstruction(action);
            ReadArgumentCollection(element, invoke.Arguments);
            instructions.Add(invoke);
        }

        protected void ReadArgumentCollection(XElement parentElement, ArgumentCollection arguments)
        {
            XElement[] elements = parentElement.Elements(Constants.Serialization.Method.Argument).ToArray();
            for (int i = 0; i < elements.Length; i++)
            {
                XElement element = elements[i];
                Argument argument = arguments[i];
                ReadArgument(element, argument);
            }
        }

        protected void ReadArgument(XElement element, Argument argument)
        {
            if (argument is InputArgument)
            {
                //Read value source at the end of parsing
                XElement valueElement = element.Element(Constants.Serialization.Method.ValueSource);
                ReadValue(valueElement, (InputArgument)argument);
            }
            else if (argument is OutputArgument)
            {
                OutputArgument outputArgument = (OutputArgument)argument;
                string valueName = element.ReadAttribute<string>(Constants.Serialization.Method.Name);
                outputArgument.Rename(valueName);
            }
        }

        protected void ReadValue(XElement element, InputArgument inputArgument)
        {
            if (element == null)
            {
                return;
            }
            XElement variableElement = element.Element(Constants.Serialization.Method.Variable);
            if (variableElement != null)
            {
                string valueName = variableElement.ReadAttribute<string>(Constants.Serialization.Method.ValueName);
                TypeReference valueType = ReadTypeReference(variableElement);
                inputArgument.Value = new VariableValue(valueType, valueName);
                return;
            }

            XElement propertyElement = element.Element(Constants.Serialization.Method.Property);
            if (propertyElement != null)
            {
                PropertyReference propertyReference = ReadPropertyReference(propertyElement);
                inputArgument.Value = new PropertyValue(propertyReference);
            }

            XElement constantElement = element.Element(Constants.Serialization.Method.Constant);
            if (constantElement != null)
            {
                TypeReference valueType = ReadTypeReference(constantElement);
                object value = _typeService.ReadValue(valueType, constantElement);
                inputArgument.Value = new ConstantValue(valueType, value);
            }
        }

        protected void ReadParameterCollection(XElement parentElement, Action action)
        {
            if (parentElement == null)
            {
                return;
            }
            IEnumerable<XElement> parameterElements = parentElement.Elements(Constants.Serialization.Method.Parameter);
            foreach (XElement parameterElement in parameterElements)
            {
                ReadParameter(parameterElement, action);
            }
        }

        protected void ReadParameter(XElement element, Action action)
        {
            ParameterDirection direction = element.ReadAttribute<ParameterDirection>(Constants.Serialization.Method.Direction);
            string parameterName = element.ReadAttribute<string>(Constants.Serialization.Method.Name);
            TypeReference valueType = ReadTypeReference(element);
            switch (direction)
            {
                case ParameterDirection.Input:
                    {
                        InputParameter parameter = new InputParameter(parameterName, valueType);
                        action.Parameters.Add(parameter);
                    }
                    break;
                case ParameterDirection.Output:
                    {
                        OutputParameter parameter = new OutputParameter(parameterName, valueType);
                        action.Parameters.Add(parameter);
                    }
                    break;
                default:
                    throw TempDesignException.UnknownEnumValue(direction);
            }
        }

        protected void ReadTitle(XElement parentElement, MethodTitle title)
        {
            if (parentElement == null)
            {
                return;
            }
            IEnumerable<XElement> titleTextElements = parentElement.Elements(Constants.Serialization.Method.TitleText);
            IEnumerator<XElement> titleTextElementsEnumerator = titleTextElements.GetEnumerator();
            foreach (TitleText titleText in title.OfType<TitleText>())
            {
                if (titleTextElementsEnumerator.MoveNext())
                {
                    XElement element = titleTextElementsEnumerator.Current;
                    titleText.Text = element.Value;
                }
            }
        }

        protected void WriteParameterCollection(XElement parentElement, Action action)
        {
            XElement parametersElement = new XElement(Constants.Serialization.Method.Parameters);
            foreach (Parameter parameter in action.Parameters)
            {
                WriteParameter(parametersElement, parameter);
            }
            parentElement.Add(parametersElement);
        }

        protected void WriteParameter(XElement parentElement, Parameter parameter)
        {
            XElement element = new XElement(Constants.Serialization.Method.Parameter);
            element.WriteAttribute<string>(Constants.Serialization.Method.Name, parameter.ValueName);
            WriteTypeReference(element, parameter.ValueType);
            if (parameter is InputParameter)
            {
                element.WriteAttribute<ParameterDirection>(Constants.Serialization.Method.Direction, ParameterDirection.Input);
            }
            else if (parameter is OutputParameter)
            {
                element.WriteAttribute<ParameterDirection>(Constants.Serialization.Method.Direction, ParameterDirection.Output);
            }
            else
            {
                throw TempDesignException.NotSupported();
            }
            parentElement.Add(element);
        }

        protected void WriteTitle(XElement parentElement, MethodTitle title)
        {
            XElement titleElement = new XElement(Constants.Serialization.Method.Title);
            foreach (TitleText titleText in title.OfType<TitleText>())
            {
                WriteTitleText(titleElement, titleText);
            }
            parentElement.Add(titleElement);
        }

        protected void WriteInstructionCollection(XElement parentElement, Method method)
        {
            XElement instructionsElement = new XElement(Constants.Serialization.Method.Instructions);
            foreach (Instruction instruction in method.Instructions)
            {
                WriteInstruction(instructionsElement, instruction);
            }
            parentElement.Add(instructionsElement);
        }

        protected void WriteInstruction(XElement parentElement, Instruction instruction)
        {
            if (instruction is InvokeInstruction)
            {
                WriteInvokeInstruction(parentElement, (InvokeInstruction)instruction);
            }
        }

        protected virtual void WriteInvokeInstruction(XElement parentElement, InvokeInstruction invoke)
        {
            XElement element = new XElement(Constants.Serialization.Method.Invoke);
            WriteMethodReference(element, invoke.Method.Reference);
            foreach (Argument argument in invoke.Arguments)
            {
                WriteArgument(element, argument);
            }
            parentElement.Add(element);
        }

        protected void WriteArgument(XElement parentElement, Argument argument)
        {
            XElement element = new XElement(Constants.Serialization.Method.Argument);
            if (argument is InputArgument)
            {
                InputArgument inputArgument = (InputArgument)argument;
                if (inputArgument.Value != null)
                {
                    WriteValue(element, inputArgument.Value);
                }
            }
            if (argument is OutputArgument)
            {
                OutputArgument outputArgument = (OutputArgument)argument;
                element.WriteAttribute(Constants.Serialization.Method.Name, outputArgument.ValueName);
            }
            parentElement.Add(element);
        }

        protected void WriteValue(XElement parentElement, BaseValue value)
        {
            XElement element = new XElement(Constants.Serialization.Method.ValueSource);
            if (value is VariableValue)
            {
                VariableValue variable = (VariableValue)value;
                XElement variableElement = new XElement(Constants.Serialization.Method.Variable);
                variableElement.WriteAttribute(Constants.Serialization.Method.ValueName, variable.Name);
                WriteTypeReference(variableElement, value.Type);
                element.Add(variableElement);
            }
            else if (value is PropertyValue)
            {
                PropertyValue property = (PropertyValue)value;
                XElement propertyElement = new XElement(Constants.Serialization.Method.Property);
                WritePropertyReference(propertyElement, property.Reference);
                element.Add(propertyElement);
            }
            else if (value is ConstantValue)
            {
                ConstantValue constantValue = (ConstantValue)value;
                XElement constantElement = new XElement(Constants.Serialization.Method.Constant);
                WriteTypeReference(constantElement, constantValue.Type);
                _typeService.WriteValue(constantValue.Type, constantValue.Value, constantElement);
                element.Add(constantValue);
            }
            parentElement.Add(element);
        }
    }
}
