using Atom.Design.Hosting;
using Atom.Design.Reflection.Code.Services;
using Atom.Design.Reflection.Metadata;
using System;

namespace Atom.Design.Reflection.Code
{
    public sealed class Assembly : IAssembly, IEquatable<Assembly>
    {
        private readonly IProject _project;

        public Assembly(IProject project, ICodeParser codeParser)
        {
            _project = project;
            Reference = new AssemblyReference(_project.AssemblyName);
            Actions = new ActionCollection(project, codeParser);
            Conditions = new ConditionCollection(project, codeParser);
            Tables = new TableCollection(project, codeParser);
        }

        public AssemblyReference Reference { get; private set; }

        public IActionCollection Actions { get; private set; }

        public IConditionCollection Conditions { get; private set; }

        public ITableCollection Tables { get; private set; }

        public bool Equals(Assembly other)
        {
            if (other == null)
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Reference.Equals(Reference);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Assembly);
        }

        public override int GetHashCode()
        {
            return Reference.GetHashCode();
        }
    }
}
