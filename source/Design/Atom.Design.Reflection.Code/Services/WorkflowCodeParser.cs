using Atom.Design.Reflection.Metadata;
using Atom.Design.Reflection.Metadata.Code;
using Atom.Runtime;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Immutable;

namespace Atom.Design.Reflection.Code.Services
{
    public sealed class WorkflowCodeParser : MethodCodeParser
    {
        protected override bool TryParseItem<T>(IMethodSymbol methodSymbol, out T item)
        {
            item = default(T);
            if (typeof(T) != typeof(IWorkflow))
            {
                return false;
            }
            WorkflowMethodAttribute attribute;
            if (!TryGetWorkflowMethodAttribute(methodSymbol, out attribute))
            {
                return false;
            }
            MethodReference methodReference = MetadataProvider.GetReference(methodSymbol);
            item = (T)(object)new Workflow(attribute.Title, methodReference);
            return true;
        }

        private bool TryGetWorkflowMethodAttribute(IMethodSymbol methodSymbol, out WorkflowMethodAttribute workflowMethodAttribute)
        {
            foreach (AttributeData attributeData in methodSymbol.GetAttributes())
            {
                try
                {
                    TypeReference attributeTypeRefence = MetadataProvider.GetReference(attributeData.AttributeClass);
                    if (!string.Equals(attributeTypeRefence.FullName, typeof(WorkflowMethodAttribute).FullName, StringComparison.Ordinal))
                    {
                        continue;
                    }
                    ImmutableArray<TypedConstant> arguments = attributeData.ConstructorArguments;
                    TypedConstant titleArgument = arguments[0];
                    workflowMethodAttribute = new WorkflowMethodAttribute((string)titleArgument.Value);
                    return true;
                }
                catch (Exception)
                {
                    //Log exception
                }
            }
            workflowMethodAttribute = null;
            return false;
        }
    }
}
