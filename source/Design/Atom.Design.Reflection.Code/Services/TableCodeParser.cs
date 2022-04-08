using Atom.Design.Hosting;
using Atom.Design.Reflection.Metadata;
using Atom.Design.Reflection.Metadata.Code;
using Atom.Runtime;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Atom.Design.Reflection.Code.Services
{
    public sealed class TableCodeParser : ICodeParser
    {
        public IList<T> Parse<T>(IProject project)
        {
            throw new NotImplementedException();
        }

        public IList<T> Parse<T>(IDocument document)
        {
            List<T> collection = new List<T>();
            if (typeof(T) != typeof(ITable))
            {
                return collection;
            }
            CSharpCompilation compilation = (CSharpCompilation)document.Project.GetCompilation();
            SyntaxNode syntaxRoot = (SyntaxNode)document.GetSyntaxRoot();
            if (syntaxRoot == null)
            {
                return collection;
            }
            SemanticModel semanticModel = null;
            foreach (TypeDeclarationSyntax typeDeclaration in syntaxRoot.DescendantNodes().OfType<TypeDeclarationSyntax>())
            {
                semanticModel = semanticModel ?? compilation.GetSemanticModel(syntaxRoot.SyntaxTree);
                ITypeSymbol typeSymbol = semanticModel.GetDeclaredSymbol(typeDeclaration);
                if (TryLoadDataTable(typeSymbol, out ITable table))
                {
                    collection.Add((T)table);
                }
            }
            return collection;
        }

        private bool TryLoadDataTable(ITypeSymbol typeSymbol, out ITable table)
        {
            table = null;
            if (!TryGetDataTableAttribute(typeSymbol, out DataTableAttribute attribute))
            {
                return false;
            }
            TypeReference typeReference = MetadataProvider.GetReference(typeSymbol);
            Dictionary<string, ITableValue> values = new Dictionary<string, ITableValue>();
            if (!TryGetDataTableValues(typeSymbol, values))
            {
                return false;
            }
            table = new Table(attribute.Title, typeReference, values);
            return true;
        }

        private bool TryGetDataTableAttribute(ITypeSymbol typeSymbol, out DataTableAttribute tableAttribute)
        {
            foreach (AttributeData attributeData in typeSymbol.GetAttributes())
            {
                try
                {
                    TypeReference attributeTypeReference = MetadataProvider.GetReference(attributeData.AttributeClass);
                    if (!string.Equals(attributeTypeReference.FullName, typeof(DataTableAttribute).FullName, StringComparison.Ordinal))
                    {
                        continue;
                    }
                    ImmutableArray<TypedConstant> arguments = attributeData.ConstructorArguments;
                    TypedConstant nameArgument = arguments[0];
                    tableAttribute = new DataTableAttribute((string)nameArgument.Value);
                    return true;
                }
                catch (Exception)
                {
                    //Log exception
                }
            }
            tableAttribute = null;
            return false;
        }

        private bool TryGetDataTableValues(ITypeSymbol typeSymbol, Dictionary<string, ITableValue> values)
        {
            foreach (IPropertySymbol propertySymbol in typeSymbol.GetMembers().OfType<IPropertySymbol>())
            {
                PropertyReference propertyReference = MetadataProvider.GetReference(propertySymbol);
                ITableValue tableValue = new TableValue(propertyReference);
                values[propertyReference.Name] = tableValue;
            }
            return true;
        }
    }
}
