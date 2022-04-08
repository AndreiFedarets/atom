using Atom.Design.Reflection.Metadata;
using Atom.Design.Reflection.Metadata.Code;
using Atom.Runtime;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Immutable;

namespace Atom.Design.Reflection.Code.Services
{
    public sealed class ActionCodeParser : MethodCodeParser
    {
        protected override bool TryParseItem<T>(IMethodSymbol methodSymbol, out T item)
        {
            item = default(T);
            if (typeof(T) != typeof(IAction))
            {
                return false;
            }
            ActionMethodAttribute attribute;
            if (!TryGetActionMethodAttribute(methodSymbol, out attribute))
            {
                return false;
            }
            MethodReference methodReference = MetadataProvider.GetReference(methodSymbol);
            item = (T)(object)new Action(attribute.Title, methodReference);
            return true;
        }

        private bool TryGetActionMethodAttribute(IMethodSymbol methodSymbol, out ActionMethodAttribute actionMethodAttribute)
        {
            foreach (AttributeData attributeData in methodSymbol.GetAttributes())
            {
                try
                {
                    TypeReference attributeTypeRefence = MetadataProvider.GetReference(attributeData.AttributeClass);
                    if (!string.Equals(attributeTypeRefence.FullName, typeof(ActionMethodAttribute).FullName, StringComparison.Ordinal))
                    {
                        continue;
                    }
                    ImmutableArray<TypedConstant> arguments = attributeData.ConstructorArguments;
                    TypedConstant titleArgument = arguments[0];
                    actionMethodAttribute = new ActionMethodAttribute((string)titleArgument.Value);
                    return true;
                }
                catch (Exception)
                {
                    //Log exception
                }
            }
            actionMethodAttribute = null;
            return false;
        }
    }
}
