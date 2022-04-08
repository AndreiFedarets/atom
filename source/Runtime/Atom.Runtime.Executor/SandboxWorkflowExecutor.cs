using System;
using System.IO;

namespace Atom.Runtime.Executor
{
    public static class SandboxWorkflowExecutor
    {
        public static void Execute(string assemblyFileFullName, string typeFullName, string methodName)
        {
            string friendlyName = typeof(WorkflowExecutor).FullName;
            AppDomainSetup setup = new AppDomainSetup();
            setup.ApplicationBase = Path.GetDirectoryName(assemblyFileFullName);
            AppDomain appDomain = AppDomain.CreateDomain(friendlyName, null, setup);
            Type executorType = typeof(WorkflowExecutor);
            WorkflowExecutor workflowExecutor = (WorkflowExecutor)appDomain.CreateInstanceAndUnwrap(executorType.Assembly.FullName, executorType.FullName);
            workflowExecutor.ExecuteSafe(assemblyFileFullName, typeFullName, methodName);
        }
    }
}
