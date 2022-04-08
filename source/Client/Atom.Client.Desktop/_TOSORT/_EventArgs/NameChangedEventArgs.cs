using System;

namespace Atom.Design
{
    public sealed class NameChangedEventArgs : EventArgs
    {
        public NameChangedEventArgs(string oldName, string newName)
        {
            OldName = oldName;
            NewName = newName;
        }

        public string OldName { get; private set; }

        public string NewName { get; private set; }

        internal static void RaiseEvent(EventHandler<NameChangedEventArgs> @event, object sender, string oldName, string newName)
        {
            if (@event != null)
            {
                @event(sender, new NameChangedEventArgs(oldName, newName));
            }
        }
    }
}
