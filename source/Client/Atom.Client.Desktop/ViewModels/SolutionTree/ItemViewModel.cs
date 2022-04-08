namespace Atom.Client.Desktop.ViewModels.SolutionTree
{
    public abstract class ItemViewModel
    {
        public ItemViewModel(ItemViewModel parent)
        {
            Parent = parent;
        }

        public ItemViewModel Parent { get; }

        public abstract string Name { get; }

        //public string FullName
        //{
        //    get
        //    {
        //        string parentFullName = Parent == null ? string.Empty : Parent.FullName;
        //        return string.Concat(parentFullName, "\\", Name); 
        //    }
        //}
    }
}
