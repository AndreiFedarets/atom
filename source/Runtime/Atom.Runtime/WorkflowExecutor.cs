using System;
using System.Reflection;

namespace Atom.Runtime
{
    public sealed class WorkflowExecutor : MarshalByRefObject
    {
        public void Execute(string assemblyFileFullName, string typeFullName, string methodName)
        {
            Assembly targetAssembly = Assembly.LoadFile(assemblyFileFullName);
            Type targetType = targetAssembly.GetType(typeFullName);
            MethodInfo targetMethod = targetType.GetMethod(methodName);
            object target = Activator.CreateInstance(targetType);
            targetMethod.Invoke(target, null);
        }

        public void ExecuteSafe(string assemblyFileFullName, string typeFullName, string methodName)
        {
            try
            {
                Execute(assemblyFileFullName, typeFullName, methodName);
            }
            catch (Exception)
            {
            }
        }
    }
}
