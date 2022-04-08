namespace Atom
{
    public interface IWorkflow
    {
        string Name { get; }

        string Description { get; }

        IActionInstanceCollection Actions { get; }
    }
}
