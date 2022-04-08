using System;

namespace Atom.Design
{
    public sealed class SolutionEventArgs : EventArgs
    {
        public SolutionEventArgs(ISolution solution)
        {
            Solution = solution;
        }

        public ISolution Solution { get; private set; }

        internal  static void RaiseEvent(EventHandler<SolutionEventArgs> @event, object sender, ISolution solution)
        {
            if (@event != null)
            {
                @event(sender, new SolutionEventArgs(solution));
            }
        }
    }
}
