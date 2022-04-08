using System;

namespace Atom.Design
{
    public sealed class AssemblyReferenceEventArgs : EventArgs
    {
        public AssemblyReferenceEventArgs(IAssemblyReference reference)
        {
            Reference = reference;
        }

        public IAssemblyReference Reference { get; private set; }

        internal static void RaiseEvent(EventHandler<AssemblyReferenceEventArgs> @event, object sender, IAssemblyReference reference)
        {
            if (@event != null)
            {
                @event(sender, new AssemblyReferenceEventArgs(reference));
            }
        }
    }
}
