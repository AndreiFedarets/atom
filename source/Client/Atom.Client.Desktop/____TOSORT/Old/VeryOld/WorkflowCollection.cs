using System.Collections.Generic;
using Atom.Rendering;

namespace Atom
{
    internal sealed class WorkflowCollection : IWorkflowCollection
    {
        private readonly List<IWorkflow> _workflows;
        private readonly IActionMessageRendering _rendering;

        public WorkflowCollection(IActionMessageRendering rendering)
        {
            _rendering = rendering;
            _workflows = new List<IWorkflow>();
        }

        public IWorkflow Create(string name, string description)
        {
            Workflow workflow = new Workflow(name, description, _rendering);
            lock (_workflows)
            {
                _workflows.Add(workflow);
            }
            return workflow;
        }

        public IEnumerator<IWorkflow> GetEnumerator()
        {
            List<IWorkflow> workflows;
            lock (_workflows)
            {
                workflows = new List<IWorkflow>();
            }
            return workflows.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
