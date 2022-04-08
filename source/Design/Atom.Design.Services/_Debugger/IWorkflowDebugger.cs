using Atom.Design.Hosting;

namespace Atom.Design.Services
{
    public interface IWorkflowDebugger
    {
        bool StartDebugging(IDocument document);

        bool StartDebugging(Workflow workflow);

        bool StartExecution(IDocument document);

        bool StartExecution(Workflow workflow);
    }
}
