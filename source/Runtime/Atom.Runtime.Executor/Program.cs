using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace Atom.Runtime.Executor
{
    static class Program
    {
        [STAThread]
        static void Main(params string[] args)
        {
            if (args == null || args.Length == 0)
            {
                return;
            }
            try
            {
                string typeFullName = string.Empty;
                string methodName = string.Empty;
                string assemblyFullName = string.Empty;
                bool waitForDebugger = false;
                for (int i = 0; i < args.Length; i++)
                {
                    string currentArgument = args[i];
                    if (string.Equals(currentArgument, "--assembly", StringComparison.Ordinal))
                    {
                        i++;
                        assemblyFullName = currentArgument;
                    }
                    else if (string.Equals(currentArgument, "--type", StringComparison.Ordinal))
                    {
                        i++;
                        typeFullName = currentArgument;
                    }
                    else if (string.Equals(currentArgument, "--method", StringComparison.Ordinal))
                    {
                        i++;
                        methodName = currentArgument;
                    }
                    else if (string.Equals(currentArgument, "--waitfordebugger", StringComparison.Ordinal))
                    {
                        i++;
                        waitForDebugger = true;
                    }
                }
                if (waitForDebugger)
                {
                    TimeSpan maxWait = TimeSpan.FromSeconds(20);
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    while (!Debugger.IsAttached && stopwatch.Elapsed < maxWait)
                    {
                        Thread.Sleep(200);
                    }
                }
                SandboxWorkflowExecutor.Execute(assemblyFullName, typeFullName, methodName);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        
    }
}
