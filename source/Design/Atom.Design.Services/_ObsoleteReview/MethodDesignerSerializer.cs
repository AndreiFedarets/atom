using Atom.Design.Hosting;
using Atom.Design.Reflection;
using Atom.Design.Reflection.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Atom.Design.Services
{
    public abstract class MethodDesignerSerializer : ObjectDesignerSerializer
    {
        private readonly IAssemblyManager _assemblyManager;

        public MethodDesignerSerializer(IAssemblyManager assemblyManager)
        {
            _assemblyManager = assemblyManager;
        }

        protected void ReadMethod(XElement parentElement, Method method, IProject context)
        {
            XElement instancesElement = parentElement.Element(Constants.Serialization.Method.Instructions);
            ReadInstructionCollection(instancesElement, method, context);
            XElement parametersElement = parentElement.Element(Constants.Serialization.Method.Parameters);
            ReadParameterCollection(parametersElement, method);
            ReadValueSourceCollection(instancesElement, method, context);
            XElement titleElement = parentElement.Element(Constants.Serialization.Method.Title);
            ReadTitle(titleElement, method.Title);
        }

        private void ReadValueSourceCollection(XElement instancesElement, Method method, IProject context)
        {
            if (instancesElement == null)
            {
                return;
            }
            XElement[] invokeElements = instancesElement.Elements(Constants.Serialization.Method.Invoke).ToArray();
            Invoke[] invokes = method.OfType<Invoke>().ToArray();
            //it is not possible that invokeElements.Length != invokes.Lengts as we just build invokes from invokeElements
            for (int i = 0; i < invokes.Length; i++)
            {
                Invoke invoke = invokes[i];
                XElement invokeElement = invokeElements[i];
                ArgumentCollection arguments = invoke.Arguments;
                XElement[] argumentElements = invokeElement.Elements(Constants.Serialization.Method.Argument).ToArray();
                for (int j = 0; j < argumentElements.Length; j++)
                {
                    Argument argument = arguments[j];
                    XElement argumentElement = argumentElements[j];
                    if (argument is IInputArgument)
                    {
                        XElement valueSourceElement = argumentElement.Element(Constants.Serialization.Method.ValueSource);
                        if (valueSourceElement != null)
                        {
                            ReadValueSource(valueSourceElement, (IInputArgument)argument, method, context);
                        }
                    }
                }
            }
        }

        private void ReadParameterCollection(XElement parentElement, Method method)
        {
            if (parentElement == null)
            {
                return;
            }
            IEnumerable<XElement> parameterElements = parentElement.Elements(Constants.Serialization.Method.Parameter);
            foreach (XElement parameterElement in parameterElements)
            {
                ReadParameter(parameterElement, method);
            }
        }

        private void ReadParameter(XElement element, Method method)
        {
            ParameterDirection direction = element.ReadAttribute<ParameterDirection>(Constants.Serialization.Method.Direction);
            string parameterName = element.ReadAttribute<string>(Constants.Serialization.Method.Name);
            switch (direction)
            {
                case ParameterDirection.Input:
                    {
                        TypeReference typeReference = ReadTypeReference(element);
                        InputParameter parameter = new InputParameter(parameterName, typeReference);
                        method.Parameters.Add(parameter);
                    }
                    break;
                case ParameterDirection.Output:
                    {
                        OutputArgument argument = DesignerHelpers.FindOutputArgument(method, parameterName);
                        OutputParameter parameter = new OutputParameter(argument);
                        method.Parameters.Add(parameter);
                    }
                    break;
                case ParameterDirection.Reference:
                    {
                        ReferenceArgument argument = DesignerHelpers.FindReferenceArgument(method, parameterName);
                        ReferenceParameter parameter = new ReferenceParameter(argument);
                        method.Parameters.Add(parameter);
                    }
                    break;
                default:
                    throw TempDesignException.UnknownEnumValue(direction);
            }
        }

        private void ReadTitle(XElement parentElement, MethodTitle title)
        {
            if (parentElement == null)
            {
                return;
            }
            IEnumerable<XElement> titleBlockElements = parentElement.Elements(Constants.Serialization.Method.TitleBlock);
            IEnumerator<XElement> titleBlockElementsEnumerator = titleBlockElements.GetEnumerator();
            foreach (TitleText titleText in title.OfType<TitleText>())
            {
                if (titleBlockElementsEnumerator.MoveNext())
                {
                    XElement element = titleBlockElementsEnumerator.Current;
                    titleText.Text = element.Value;
                }
            }
        }

        private void ReadInstructionCollection(XElement parentElement, Method method, IProject context)
        {
            if (parentElement == null)
            {
                return;
            }
            foreach (XElement instructionElement in parentElement.Elements())
            {
                ReadInstruction(instructionElement, method, context);
            }
        }

        private void ReadInstruction(XElement element, Method method, IProject context)
        {
            if (string.Equals(element.Name.LocalName, Constants.Serialization.Method.Invoke, StringComparison.Ordinal))
            {
                ReadInvoke(element, method, context);
            }
        }

        private void ReadInvoke(XElement element, Method method, IProject context)
        {
            MethodReference methodReference = ReadMethodReference(element);
            IAssembly assembly = _assemblyManager.GetAssembly(methodReference.DeclaringType.Assembly, context);
            IAction action = assembly.Actions[methodReference];
            Invoke invoke = new Invoke(action);
            ReadArgumentCollection(element, invoke.Arguments);
            method.Add(invoke);
        }

        private void ReadArgumentCollection(XElement parentElement, ArgumentCollection arguments)
        {
            XElement[] elements = parentElement.Elements(Constants.Serialization.Method.Argument).ToArray();
            for (int i = 0; i < elements.Length; i++)
            {
                XElement element = elements[i];
                Argument argument = arguments[i];
                ReadArgument(element, argument);
            }
        }

        private void ReadArgument(XElement element, Argument argument)
        {
            if (argument is IInputArgument)
            {
                //Read value source at the end of parsing
            }
            if (argument is IOutputArgument)
            {
                IOutputArgument outputArgument = (IOutputArgument)argument;
                outputArgument.ValueName = element.ReadAttribute<string>(Constants.Serialization.Method.Name);
            }
        }

        private void ReadValueSource(XElement element, IInputArgument inputArgument, Method method, IProject context)
        {
            XElement outputArgumentElement = element.Element(Constants.Serialization.Method.OutputArgument);
            if (outputArgumentElement != null)
            {
                string variableName = outputArgumentElement.ReadAttribute<string>(Constants.Serialization.Method.VariableName);
                inputArgument.Source = DesignerHelpers.FindOutputValueSource(method, variableName);
            }

            XElement inputParameterElement = element.Element(Constants.Serialization.Method.InputParameter);
            if (inputParameterElement != null)
            {
                string parameterName = inputParameterElement.ReadAttribute<string>(Constants.Serialization.Method.VariableName);
                inputArgument.Source = DesignerHelpers.FindInputParameter(method, parameterName);
            }

            XElement tableValueElement = element.Element(Constants.Serialization.Method.TableValue);
            if (tableValueElement != null)
            {
                string variableName = tableValueElement.ReadAttribute<string>(Constants.Serialization.Method.VariableName);
                TypeReference declaringType = ReadTypeReference(tableValueElement);
                IAssembly assembly = _assemblyManager.GetAssembly(declaringType.Assembly, context);
                ITable table = assembly.Tables[declaringType];
                inputArgument.Source = table[variableName];
            }
        }

        protected void WriteMethod(XElement parentElement, Method method)
        {
            WriteTitle(parentElement, method.Title);
            WriteParameterCollection(parentElement, method);
            WriteInstructionCollection(parentElement, method);
        }

        private void WriteParameterCollection(XElement parentElement, Method method)
        {
            XElement parametersElement = new XElement(Constants.Serialization.Method.Parameters);
            foreach (Parameter parameter in method.Parameters)
            {
                WriteParameter(parametersElement, parameter);
            }
            parentElement.Add(parametersElement);
        }

        private void WriteParameter(XElement parentElement, Parameter parameter)
        {
            XElement element = new XElement(Constants.Serialization.Method.Parameter);
            if (parameter is InputParameter)
            {
                InputParameter inputParameter = (InputParameter)parameter;
                element.WriteAttribute<ParameterDirection>(Constants.Serialization.Method.Direction, ParameterDirection.Input);
                element.WriteAttribute<string>(Constants.Serialization.Method.Name, inputParameter.ValueName);
                WriteTypeReference(element, inputParameter.ValueType);
            }
            else if (parameter is OutputParameter)
            {
                OutputParameter outputParameter = (OutputParameter)parameter;
                element.WriteAttribute<ParameterDirection>(Constants.Serialization.Method.Direction, ParameterDirection.Output);
                element.WriteAttribute<string>(Constants.Serialization.Method.Name, outputParameter.SourceArgument.ValueName);
            }
            else if (parameter is ReferenceParameter)
            {
                ReferenceParameter referenceParameter = (ReferenceParameter)parameter;
                element.WriteAttribute<ParameterDirection>(Constants.Serialization.Method.Direction, ParameterDirection.Reference);
                element.WriteAttribute<string>(Constants.Serialization.Method.Name, referenceParameter.ValueName);
            }
            else
            {
                throw TempDesignException.NotSupported();
            }
            parentElement.Add(element);
        }

        private void WriteTitle(XElement parentElement, MethodTitle title)
        {
            XElement titleElement = new XElement(Constants.Serialization.Method.Title);
            foreach (TitleText titleText in title.OfType<TitleText>())
            {
                XElement titleBlockElement = new XElement(Constants.Serialization.Method.TitleBlock);
                titleBlockElement.Value = titleText.Text;
                titleElement.Add(titleBlockElement);
            }
            parentElement.Add(titleElement);
        }

        private void WriteInstructionCollection(XElement parentElement, Method method)
        {
            XElement instancesElement = new XElement(Constants.Serialization.Method.Instructions);
            foreach (Instruction instruction in method.OfType<Instruction>())
            {
                WriteInstruction(instancesElement, instruction);
            }
            parentElement.Add(instancesElement);
        }

        private void WriteInstruction(XElement parentElement, Instruction instruction)
        {
            if (instruction is Invoke)
            {
                WriteInvoke(parentElement, (Invoke)instruction);
            }
        }

        private void WriteInvoke(XElement parentElement, Invoke invoke)
        {
            XElement element = new XElement(Constants.Serialization.Method.Invoke);
            WriteMethodReference(element, invoke.Method.Method);
            foreach (Argument argument in invoke.Arguments)
            {
                WriteArgument(element, argument);
            }
            parentElement.Add(element);
        }

        private void WriteArgument(XElement parentElement, Argument argument)
        {
            XElement element = new XElement(Constants.Serialization.Method.Argument);
            if (argument is IInputArgument)
            {
                IInputArgument inputArgument = (IInputArgument)argument;
                if (inputArgument.Source != null)
                {
                    WriteValueSource(element, inputArgument.Source);
                }
            }
            if (argument is IOutputArgument)
            {
                IOutputArgument outputArgument = (IOutputArgument)argument;
                element.WriteAttribute(Constants.Serialization.Method.Name, outputArgument.ValueName);
            }
            parentElement.Add(element);
        }

        private void WriteValueSource(XElement parentElement, IValueSource valueSource)
        {
            XElement element = new XElement(Constants.Serialization.Method.ValueSource);
            if (valueSource is IOutputArgument)
            {
                XElement outputArgumentElement = new XElement(Constants.Serialization.Method.OutputArgument);
                outputArgumentElement.WriteAttribute(Constants.Serialization.Method.VariableName, valueSource.ValueName);
                element.Add(outputArgumentElement);
            }
            else if (valueSource is InputParameter)
            {
                XElement inputParameterElement = new XElement(Constants.Serialization.Method.InputParameter);
                inputParameterElement.WriteAttribute<string>(Constants.Serialization.Method.VariableName, valueSource.ValueName);
                element.Add(inputParameterElement);
            }
            else if (valueSource is ReferenceParameter)
            {
                XElement referenceParameterElement = new XElement(Constants.Serialization.Method.ReferenceParameter);
                referenceParameterElement.WriteAttribute<string>(Constants.Serialization.Method.VariableName, valueSource.ValueName);
                element.Add(referenceParameterElement);
            }
            else if (valueSource is ITableValue)
            {
                XElement tableValueElement = new XElement(Constants.Serialization.Method.TableValue);
                tableValueElement.WriteAttribute(Constants.Serialization.Method.VariableName, valueSource.ValueName);
                ITableValue tableValue = (ITableValue)valueSource;
                WriteTypeReference(tableValueElement, tableValue.Property.DeclaringType);
                element.Add(tableValueElement);
            }
            parentElement.Add(element);
        }
    }
}
