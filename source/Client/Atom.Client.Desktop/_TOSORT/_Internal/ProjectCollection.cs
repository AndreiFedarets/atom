using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Atom.Design
{
    internal sealed class ProjectCollection : ReadOnlyCollection<IProject>, IProjectCollection
    {
        //private readonly IAssemblyManager _assemblyManager;
        private readonly ISolution _solution;

        public ProjectCollection(ISolution solution)
            : base(new List<IProject>())
        {
            _solution = solution;
            
        }

        public event EventHandler<ProjectEventArgs> ProjectAdded;

        public event EventHandler<ProjectEventArgs> ProjectRemoved;

        public IProject Create(string name)
        {
            throw new NotImplementedException();
            //Guid uid = Guid.NewGuid();
            //IProject project = new Project(uid, name, _solution);
            //Items.Add(project);
            //ProjectEventArgs.RaiseEvent(ProjectAdded, this, project);
            //return project;
        }
    }
}
