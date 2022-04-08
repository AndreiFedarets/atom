using Atom.Design.Reflection;
using Atom.Design.Reflection.Metadata;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

namespace Atom.Design.Services
{
    public abstract class MethodDesignerCodeGenerator : ObjectDesignerCodeGenerator
    {
        public MethodDesignerCodeGenerator(ITypeService typeService)
            : base(typeService)
        {
        }

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
                    CodeTypeReference typeReference = new CodeTypeReference(outputParameter.ValueType.FullName);
                    CodeParameterDeclarationExpression parameterDeclaration = new CodeParameterDeclarationExpression(typeReference, outputParameter.ValueName);
                    parameterDeclaration.Direction = FieldDirection.Out;
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
            foreach (InvokeInstruction invoke in designer.Instructions.OfType<InvokeInstruction>())
            {
                //Declare output variables
                ArgumentCollection arguments = invoke.Arguments;
                foreach (OutputArgument outputArgument in arguments.OfType<OutputArgument>())
                {
                    //skip OutputArgument if it was made OutputParameter
                    //if (IsExternalArgument(designer.Parameters, outputArgument))
                    //{
                    //    continue;
                    //}
                    CodeTypeReference typeReference = new CodeTypeReference(outputArgument.Parameter.ParameterType.FullName);
                    string name = outputArgument.ValueName;
                    CodeVariableDeclarationStatement declareVariable = new CodeVariableDeclarationStatement(typeReference, name);
                    method.Statements.Add(declareVariable);
                }
                //Invoke action
                CodeExpression[] methodArguments = new CodeExpression[invoke.Method.Reference.Parameters.Count];
                for (int i = 0; i < invoke.Method.Reference.Parameters.Count; i++)
                {
                    ParameterReference parameter = invoke.Method.Reference.Parameters[i];
                    Argument argument = invoke.Arguments[parameter];
                    switch (parameter.Direction)
                    {
                        case ParameterDirection.Input:
                            {
                                InputArgument inputArgument = (InputArgument)argument;
                                BaseValue value = inputArgument.Value;
                                methodArguments[i] = GetCodeExpressionFromValue(value);
                            }
                            break;
                        case ParameterDirection.Output:
                            {
                                OutputArgument outputArgument = (OutputArgument)argument;
                                CodeVariableReferenceExpression variableReference = new CodeVariableReferenceExpression(outputArgument.ValueName);
                                methodArguments[i] = new CodeDirectionExpression(FieldDirection.Out, variableReference);
                            }
                            break;
                    }
                }
                CodeExpression targetObject = new CodeTypeReferenceExpression(invoke.Method.Reference.DeclaringType.FullName);
                string methodName = invoke.Method.Reference.Name;
                CodeMethodInvokeExpression invokeExpression = new CodeMethodInvokeExpression(targetObject, methodName, methodArguments);
                method.Statements.Add(invokeExpression);
            }
        }

        private CodeExpression GetCodeExpressionFromValue(BaseValue value)
        {
            CodeExpression codeExpression = null;
            if (value is VariableValue)
            {
                VariableValue variableValue = (VariableValue)value;
                codeExpression = new CodeVariableReferenceExpression(variableValue.Name);
            }
            else if (value is PropertyValue)
            {
                PropertyValue propertyValue = (PropertyValue)value;
                CodeTypeReferenceExpression typeReference = new CodeTypeReferenceExpression(propertyValue.Reference.DeclaringType.FullName);
                codeExpression = new CodePropertyReferenceExpression(typeReference, propertyValue.Reference.Name);
            }
            else if (value is ConstantValue)
            {
                ConstantValue constantValue = (ConstantValue)value;
                TypeAdapter typeAdapter = TypeService.FindAdapter(constantValue.Type);
                IEnumerable<CodeStatement> statements = typeAdapter.GenerateCode(constantValue.Value);
                throw new System.NotImplementedException();
            }
            return codeExpression;
        }

        //private bool IsExternalArgument(ParameterCollection parameters, OutputArgument outputArgument)
        //{
        //    foreach (OutputParameter outputParameter in parameters.OfType<OutputParameter>())
        //    {
        //        if (Equals(outputParameter.SourceArgument, outputArgument))
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}
    }
}
