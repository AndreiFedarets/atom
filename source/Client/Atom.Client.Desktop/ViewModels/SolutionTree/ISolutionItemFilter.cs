namespace Atom.Client.Desktop.ViewModels.SolutionTree
{
    public interface ISolutionItemFilter
    {
        bool Display(ItemViewModel item);
    }
}
