using Atom.Design.Reflection.Metadata;
using Atom.Design.Reflection.Metadata.Code;
using Atom.Runtime;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Immutable;

namespace Atom.Design.Reflection.Code.Services
{
    public sealed class ConditionCodeParser : MethodCodeParser
    {
        protected override bool TryParseItem<T>(IMethodSymbol methodSymbol, out T item)
        {
            item = default(T);
            if (typeof(T) != typeof(ICondition))
            {
                return false;
            }
            ConditionMethodAttribute attribute;
            if (methodSymbol.ReturnType.SpecialType != SpecialType.System_Boolean || !TryGetConditionMethodAttribute(methodSymbol, out attribute))
            {
                return false;
            }
            MethodReference methodReference = MetadataProvider.GetReference(methodSymbol);
            item = (T)(object)new Condition(attribute.Title, methodReference);
            return true;
        }

        private bool TryGetConditionMethodAttribute(IMethodSymbol methodSymbol, out ConditionMethodAttribute conditionMethodAttribute)
        {
            foreach (AttributeData attributeData in methodSymbol.GetAttributes())
            {
                try
                {
                    TypeReference attributeTypeRefence = MetadataProvider.GetReference(attributeData.AttributeClass);
                    if (!string.Equals(attributeTypeRefence.FullName, typeof(ConditionMethodAttribute).FullName, StringComparison.Ordinal))
                    {
                        continue;
                    }
                    ImmutableArray<TypedConstant> arguments = attributeData.ConstructorArguments;
                    TypedConstant titleArgument = arguments[0];
                    conditionMethodAttribute = new ConditionMethodAttribute((string)titleArgument.Value);
                    return true;
                }
                catch (Exception)
                {
                    //Log exception
                }
            }
            conditionMethodAttribute = null;
            return false;
        }
    }
}
