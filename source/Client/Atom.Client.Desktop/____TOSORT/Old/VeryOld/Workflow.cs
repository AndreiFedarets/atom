using Atom.Execution;
using Atom.Rendering;

namespace Atom
{
    public sealed class Workflow : IWorkflow
    {
        private volatile bool _locked;
        private string _name;
        private string _description;
        private readonly ActionInstanceCollection _actions;

        public Workflow(string name, string description, IActionMessageRendering rendering)
        {
            _locked = false;
            Name = name;
            Description = description;
            _actions = new ActionInstanceCollection(rendering);
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (_locked)
                {
                    throw new TempRuntimeException();
                }
                _name = value;
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (_locked)
                {
                    throw new TempRuntimeException();
                }
                _description = value;
            }
        }

        public IActionInstanceCollection Actions
        {
            get { return _actions; }
        }
    }
}
