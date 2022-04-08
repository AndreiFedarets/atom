using System.Collections.Generic;

namespace Atom.Design.Reflection.Metadata.Binary
{
    public static class MetadataProvider
    {
        public static AssemblyReference GetReference(System.Reflection.Assembly assembly)
        {
            return new AssemblyReference(assembly.GetName());
        }

        public static TypeReference GetReference(System.Type type)
        {
            List<TypeReference> baseTypes = new List<TypeReference>();
            foreach (System.Type interfaceType in type.GetInterfaces())
            {
                TypeReference interfaceTypeReference = GetReference(interfaceType);
                baseTypes.Add(interfaceTypeReference);
            }
            if (type.BaseType != null)
            {
                TypeReference baseTypeReference = GetReference(type.BaseType);
                baseTypes.Add(baseTypeReference);
            }
            AssemblyReference assemblyReference = GetReference(type.Assembly);
            return new TypeReference(type.Name, type.Namespace, assemblyReference, baseTypes.ToArray());
        }

        public static PropertyReference GetReference(System.Reflection.PropertyInfo propertyInfo)
        {
            TypeReference declaringTypeReference = GetReference(propertyInfo.DeclaringType);
            TypeReference valueTypeReference = GetReference(propertyInfo.PropertyType);
            return new PropertyReference(propertyInfo.Name, valueTypeReference, declaringTypeReference);
        }

        public static MethodReference GetReference(System.Reflection.MethodInfo methodInfo)
        {
            TypeReference typeReference = GetReference(methodInfo.DeclaringType);
            List<ParameterReference> parameterReferences = new List<ParameterReference>();
            foreach (System.Reflection.ParameterInfo parameterInfo in methodInfo.GetParameters())
            {
                ParameterReference parameterReference = GetReference(parameterInfo);
                parameterReferences.Add(parameterReference);
            }
            ParameterReferenceCollection parameters = new ParameterReferenceCollection(parameterReferences);
            return new MethodReference(methodInfo.Name, parameters, typeReference);
        }

        public static ParameterReference GetReference(System.Reflection.ParameterInfo parameterInfo)
        {
            ParameterDirection direction = ParameterDirection.Input;
            if (parameterInfo.IsOut)
            {
                direction = ParameterDirection.Output;
            }
            else if (parameterInfo.ParameterType.IsByRef)
            {
                direction = ParameterDirection.Reference;
            }
            //TODO: handle ActionArgumentAttribute.Name
            System.Type parameterType = direction.HasFlag(ParameterDirection.Output) ? parameterInfo.ParameterType.GetElementType() : parameterInfo.ParameterType;
            TypeReference parameterTypeReference = GetReference(parameterType);
            return new ParameterReference(parameterInfo.Name, direction, parameterTypeReference);
        }
    }
}
