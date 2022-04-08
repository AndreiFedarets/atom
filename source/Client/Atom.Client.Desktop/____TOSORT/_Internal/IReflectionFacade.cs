using System;
using System.Collections.Generic;
using System.Reflection;

namespace Atom
{
    internal interface IReflectionFacade
    {
        IList<IParameter> BuildParameters(MethodInfo methodInfo);

        void VerifyMethodArguments(MethodInfo methodInfo, object[] arguments);

        void InvokeMethod(MethodInfo methodInfo, object[] arguments);

        string GetParameterName(ParameterInfo parameterInfo);

        Type GetParameterType(ParameterInfo parameterInfo);

        Direction GetParameterDirection(ParameterInfo parameterInfo);

        Guid GetAssemblyGuid(Assembly assembly);

        AssemblyName GetAssemblyName(Assembly assembly);

        ActionMethodAttribute GetActionMethodAttribute(MethodInfo methodInfo);

        bool IsTypeAssignableFrom(Type fromType, Type toType);

        Assembly LoadAssembly(string fileFullName);

        ActionAssemblyAttribute GetActionAssemblyAttribute(Assembly assembly);
    }
}
