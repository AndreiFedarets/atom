using Atom.Design.Reflection;
using Atom.Design.Reflection.Metadata;
using System.CodeDom;
using System.Linq;

namespace Atom.Design.Services
{
    public abstract class MethodDesignerCodeGenerator : ObjectDesignerCodeGenerator
    {
        protected void GenerateParameters(CodeMemberMethod method, ParameterCollection parameters)
        {
            foreach (Parameter parameter in parameters)
            {
                if (parameter is InputParameter)
                {
                    InputParameter inputParameter = (InputParameter)parameter;
                    CodeTypeReference typeReference = new CodeTypeReference(inputParameter.ValueType.FullName);
                    CodeParameterDeclarationExpression parameterDeclaration = new CodeParameterDeclarationExpression(typeReference, inputParameter.ValueName);
                    parameterDeclaration.Direction = FieldDirection.In;
                    method.Parameters.Add(parameterDeclaration);
                }
                else if (parameter is OutputParameter)
                {
                    OutputParameter outputParameter = (OutputParameter)parameter;
                    CodeTypeReference typeReference = new CodeTypeReference(outputParameter.SourceArgument.ValueType.FullName);
                    CodeParameterDeclarationExpression parameterDeclaration = new CodeParameterDeclarationExpression(typeReference, outputParameter.SourceArgument.ValueName);
                    parameterDeclaration.Direction = FieldDirection.Out;
                    method.Parameters.Add(parameterDeclaration);
                }
                else if (parameter is ReferenceParameter)
                {
                    ReferenceParameter referenceParameter = (ReferenceParameter)parameter;
                    CodeTypeReference typeReference = new CodeTypeReference(referenceParameter.ValueType.FullName);
                    CodeParameterDeclarationExpression parameterDeclaration = new CodeParameterDeclarationExpression(typeReference, referenceParameter.ValueName);
                    parameterDeclaration.Direction = FieldDirection.Ref;
                    method.Parameters.Add(parameterDeclaration);
                }
                else
                {
                    throw TempDesignException.NotSupported();
                }
            }
        }

        protected void GenerateMethodBody(CodeMemberMethod method, Method designer)
        {
            foreach (Invoke invoke in designer.OfType<Invoke>())
            {
                //Declare output variables
                ArgumentCollection arguments = invoke.Arguments;
                foreach (OutputArgument outputArgument in arguments.OfType<OutputArgument>())
                {
                    //skip OutputArgument if it was made OutputParameter
                    if (IsExternalArgument(designer.Parameters, outputArgument))
                    {
                        continue;
                    }
                    CodeTypeReference typeReference = new CodeTypeReference(outputArgument.Parameter.ParameterType.FullName);
                    string name = outputArgument.ValueName;
                    CodeVariableDeclarationStatement declareVariable = new CodeVariableDeclarationStatement(typeReference, name);
                    method.Statements.Add(declareVariable);
                }
                //Declare reference varaibles if needed
                foreach (ReferenceArgument referenceArgument in arguments.OfType<ReferenceArgument>())
                {
                    IValueSource valueSource = referenceArgument.Source;
                    if (valueSource != null)
                    {
                        if (valueSource is ITableValue)
                        {
                            ITableValue tableValue = (ITableValue)valueSource;
                            CodeTypeReference typeReference = new CodeTypeReference(referenceArgument.Parameter.ParameterType.FullName);
                            string name = referenceArgument.ValueName;
                            CodeExpression valueExpression = GetCodeExpressionFromValueSource(tableValue);
                            CodeVariableDeclarationStatement declareVariable = new CodeVariableDeclarationStatement(typeReference, name, valueExpression);
                            method.Statements.Add(declareVariable);
                        }
                    }
                }
                //Invoke action
                CodeExpression[] methodArguments = new CodeExpression[invoke.Method.Method.Parameters.Count];
                for (int i = 0; i < invoke.Method.Method.Parameters.Count; i++)
                {
                    ParameterReference parameter = invoke.Method.Method.Parameters[i];
                    Argument argument = invoke.Arguments[parameter];
                    switch (parameter.Direction)
                    {
                        case ParameterDirection.Input:
                            {
                                InputArgument inputArgument = (InputArgument)argument;
                                IValueSource valueSource = inputArgument.Source;
                                methodArguments[i] = GetCodeExpressionFromValueSource(valueSource);
                            }
                            break;
                        case ParameterDirection.Output:
                            {
                                OutputArgument outputArgument = (OutputArgument)argument;
                                CodeVariableReferenceExpression variableReference = new CodeVariableReferenceExpression(outputArgument.ValueName);
                                methodArguments[i] = new CodeDirectionExpression(FieldDirection.Out, variableReference);
                            }
                            break;
                        case ParameterDirection.Reference:
                            {
                                ReferenceArgument referenceArgument = (ReferenceArgument)argument;
                                CodeVariableReferenceExpression variableReference = new CodeVariableReferenceExpression(referenceArgument.ValueName);
                                methodArguments[i] = new CodeDirectionExpression(FieldDirection.Ref, variableReference);
                            }
                            break;
                    }
                }
                CodeExpression targetObject = new CodeTypeReferenceExpression(invoke.Method.Method.DeclaringType.FullName);
                string methodName = invoke.Method.Method.Name;
                CodeMethodInvokeExpression invokeExpression = new CodeMethodInvokeExpression(targetObject, methodName, methodArguments);
                method.Statements.Add(invokeExpression);
            }
        }

        private CodeExpression GetCodeExpressionFromValueSource(IValueSource valueSource)
        {
            CodeExpression codeExpression = null;
            if (valueSource is IOutputArgument)
            {
                codeExpression = new CodeVariableReferenceExpression(valueSource.ValueName);
            }
            else if (valueSource is InputParameter)
            {
                codeExpression = new CodeVariableReferenceExpression(valueSource.ValueName);
            }
            else if (valueSource is ITableValue)
            {
                ITableValue tableValue = (ITableValue)valueSource;
                CodeTypeReferenceExpression typeReference = new CodeTypeReferenceExpression(tableValue.Property.DeclaringType.FullName);
                codeExpression = new CodePropertyReferenceExpression(typeReference, valueSource.ValueName);
            }
            return codeExpression;
        }

        private bool IsExternalArgument(ParameterCollection parameters, OutputArgument outputArgument)
        {
            foreach (OutputParameter outputParameter in parameters.OfType<OutputParameter>())
            {
                if (Equals(outputParameter.SourceArgument, outputArgument))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
