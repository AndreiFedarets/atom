namespace Atom
{
    public interface IActionInstanceCollection : IReadOnlyCollection<IActionInstance>
    {
        IActionInstance Insert(int index, IActionType actionSource);

        IActionInstance Add(IActionType actionSource);
    }
}
