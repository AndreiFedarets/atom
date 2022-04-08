using Atom.Design.Hosting;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Atom.Design.Reflection.Metadata.Code
{
    public static class MetadataProvider
    {
        public static AssemblyReference GetReference(IAssemblySymbol assemblySymbol)
        {
            return new AssemblyReference(assemblySymbol.Identity.ToString());
        }

        public static TypeReference GetReference(ITypeSymbol typeSymbol)
        {
            INamespaceSymbol namespaceSymbol = typeSymbol.ContainingNamespace;
            AssemblyReference assemblyReference = GetReference(typeSymbol.ContainingAssembly);
            List<TypeReference> baseTypes = new List<TypeReference>();
            foreach (ITypeSymbol interfaceType in typeSymbol.AllInterfaces)
            {
                TypeReference interfaceTypeReference = GetReference(interfaceType);
                baseTypes.Add(interfaceTypeReference);
            }
            if (typeSymbol.BaseType != null)
            {
                TypeReference baseTypeReference = GetReference(typeSymbol.BaseType);
                baseTypes.Add(baseTypeReference);
            }
            
            return new TypeReference(typeSymbol.Name, namespaceSymbol.ToDisplayString(), assemblyReference, baseTypes.ToArray());
        }

        public static MethodReference GetReference(IMethodSymbol methodSymbol)
        {
            TypeReference typeReference = GetReference(methodSymbol.ContainingType);
            List<ParameterReference> parameterReferences = new List<ParameterReference>();
            foreach (IParameterSymbol parameterSymbol in methodSymbol.Parameters)
            {
                ParameterReference parameterReference = GetReference(parameterSymbol);
                parameterReferences.Add(parameterReference);
            }
            ParameterReferenceCollection parameters = new ParameterReferenceCollection(parameterReferences);
            return new MethodReference(methodSymbol.Name, parameters, typeReference);
        }

        public static PropertyReference GetReference(IPropertySymbol propertySymbol)
        {
            TypeReference declaringTypeReference = GetReference(propertySymbol.ContainingType);
            TypeReference valueTypeReference = GetReference(propertySymbol.Type);
            return new PropertyReference(propertySymbol.Name, valueTypeReference, declaringTypeReference);
        }

        public static ParameterReference GetReference(IParameterSymbol parameterSymbol)
        {
            ParameterDirection parameterDirection = ParameterDirection.Input;
            if (parameterSymbol.RefKind == RefKind.Out)
            {
                parameterDirection = ParameterDirection.Output;
            }
            else if (parameterSymbol.RefKind == RefKind.Ref)
            {
                parameterDirection = ParameterDirection.Reference;
            }
            TypeReference parameterTypeReference = GetReference(parameterSymbol.Type);
            ParameterReference parameterReference = new ParameterReference(parameterSymbol.Name, parameterDirection, parameterTypeReference);
            return parameterReference;
        }

        public static AssemblyReference GetReference(IProject project)
        {
            Compilation compilation = (Compilation)project.GetCompilation();
            return GetReference(compilation.Assembly);
        }
    }
}
