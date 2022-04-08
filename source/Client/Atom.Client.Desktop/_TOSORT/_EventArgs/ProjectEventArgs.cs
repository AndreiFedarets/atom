using System;

namespace Atom.Design
{
    public sealed class ProjectEventArgs : EventArgs
    {
        public ProjectEventArgs(IProject project)
        {
            Project = project;
        }

        public IProject Project { get; private set; }

        internal static void RaiseEvent(EventHandler<ProjectEventArgs> @event, object sender, IProject project)
        {
            if (@event != null)
            {
                @event(sender, new ProjectEventArgs(project));
            }
        }
    }
}
