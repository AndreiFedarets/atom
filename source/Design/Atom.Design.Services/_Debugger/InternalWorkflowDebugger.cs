using Atom.Design.Common;
using Atom.Design.Hosting;
using Atom.Design.Reflection.Metadata;
using System.Diagnostics;

namespace Atom.Design.Services
{
    internal sealed class InternalWorkflowDebugger : IWorkflowDebugger
    {
        private readonly IDesignerSerializer _serializer;
        private readonly IWorkspace _workspace;

        public InternalWorkflowDebugger(IDesignerSerializer serializer, IWorkspace workspace)
        {
            _serializer = serializer;
            _workspace = workspace;
        }

        public bool StartDebugging(IDocument document)
        {
            if (document == null)
            {
                return false;
            }
            Workflow workflow = _serializer.Read(document) as Workflow;
            if (workflow == null)
            {
                return false;
            }
            return StartDebugging(workflow);
        }

        public bool StartDebugging(Workflow workflow)
        {
            Process process = StartExecutorProcess(workflow, true);
            _workspace.Attach(process.Id);
            return true;
        }

        public bool StartExecution(IDocument document)
        {
            if (document == null)
            {
                return false;
            }
            Workflow workflow = _serializer.Read(document) as Workflow;
            if (workflow == null)
            {
                return false;
            }
            return StartExecution(workflow);
        }

        public bool StartExecution(Workflow workflow)
        {
            StartExecutorProcess(workflow, false);
            return true;
        }

        private Process StartExecutorProcess(Workflow workflow, bool debugging)
        {
            string assemblyFileFullName = workflow.Document.Project.OutputFilePath;
            string methodName = workflow.GetMethodName();
            TypeReference type = workflow.GetTypeReference();
            string commandLine = string.Format("--assembly \"{0}\" --type {1} --method {2}", assemblyFileFullName, type.FullName, methodName);
            if (debugging)
            {
                commandLine += " --waitfordebugger";
            }
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo(Environment.InternalExecutorPath)
                {
                    UseShellExecute = false,
                    Arguments = commandLine
                }
            };
            process.Start();
            return process;
        }
    }
}
