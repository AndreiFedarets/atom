using Atom.Client.Win.Commands;

namespace Atom.Client.Win.SolutionExplorer.Commands
{
    public abstract class ItemCommand : SyncCommand
    {
        public abstract string Header { get; }
    }
}
