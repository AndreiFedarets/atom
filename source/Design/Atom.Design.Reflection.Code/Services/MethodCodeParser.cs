using Atom.Design.Hosting;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atom.Design.Reflection.Code.Services
{
    public abstract class MethodCodeParser : ICodeParser
    {
        public IList<T> Parse<T>(IDocument document)
        {
            List<T> collection = new List<T>();
            Compilation compilation = document.Project.GetCompilation();
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
                foreach (IMethodSymbol methodSymbol in typeSymbol.GetMembers().OfType<IMethodSymbol>())
                {
                    T item;
                    if (TryParseItem(methodSymbol, out item))
                    {
                        collection.Add(item);
                    }
                }
            }
            return collection;
        }

        public IList<T> Parse<T>(IProject project)
        {
            throw new NotImplementedException();
        }

        protected abstract bool TryParseItem<T>(IMethodSymbol methodSymbol, out T item);
    }
}
