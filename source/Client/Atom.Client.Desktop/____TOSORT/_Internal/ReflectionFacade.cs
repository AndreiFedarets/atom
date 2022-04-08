using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Atom
{
    internal sealed class ReflectionFacade : IReflectionFacade
    {
        public IList<IParameter> BuildParameters(MethodInfo methodInfo)
        {
            List<IParameter> parameters = new List<IParameter>();
            ParameterInfo[] parameterInfos = methodInfo.GetParameters();
            foreach (ParameterInfo parameterInfo in parameterInfos)
            {
                CodedParameter parameter = new CodedParameter(parameterInfo, this);
                parameters.Add(parameter);
            }
            return parameters;
        }

        public void VerifyMethodArguments(MethodInfo methodInfo, object[] arguments)
        {
            ParameterInfo[] parameterInfos = methodInfo.GetParameters();
            if (parameterInfos.Length != arguments.Length)
            {
                throw Fail.Execution.TempException();
            }
            for (int i = 0; i < parameterInfos.Length; i++)
            {
                ParameterInfo parameterInfo = parameterInfos[i];
                if (GetParameterDirection(parameterInfo) == Direction.Output)
                {
                    continue;
                }
                object argument = arguments[i];
                //TODO: implement
            }
        }

        public void InvokeMethod(MethodInfo methodInfo, object[] arguments)
        {
            
        }

        public ActionMethodAttribute GetActionMethodAttribute(MethodInfo methodInfo)
        {
            ActionMethodAttribute attribute = null;
            object[] attributes = methodInfo.GetCustomAttributes(typeof (ActionMethodAttribute), false);
            if (attributes.Length == 1)
            {
                attribute = (ActionMethodAttribute) attributes[0];
            }
            return attribute;
        }

        public string GetParameterName(ParameterInfo parameterInfo)
        {
            return parameterInfo.Name;
        }

        public Type GetParameterType(ParameterInfo parameterInfo)
        {
            //TODO: looks bad, refactor it
            Type type = parameterInfo.ParameterType;
            if (type.Name.EndsWith("&"))
            {
                type = Type.GetType(type.AssemblyQualifiedName.Replace("&", string.Empty));
            }
            return type;
        }

        public Guid GetAssemblyGuid(Assembly assembly)
        {
            GuidAttribute attribute = (GuidAttribute) assembly.GetCustomAttributes(typeof (GuidAttribute), true)[0];
            return new Guid(attribute.Value);
        }

        public AssemblyName GetAssemblyName(Assembly assembly)
        {
            return assembly.GetName();
        }


        public bool IsTypeAssignableFrom(Type fromType, Type toType)
        {
            return toType.IsAssignableFrom(fromType);
        }

        public Assembly LoadAssembly(string fileFullName)
        {
            Assembly assembly = Assembly.LoadFile(fileFullName);
            return assembly;
        }

        public ActionAssemblyAttribute GetActionAssemblyAttribute(Assembly assembly)
        {
            object[] collection = assembly.GetCustomAttributes(typeof (ActionAssemblyAttribute), false);
            ActionAssemblyAttribute attribute = null;
            if (collection.Length == 1)
            {
                attribute = (ActionAssemblyAttribute) collection[0];
            }
            return attribute;
        }
    }
}
