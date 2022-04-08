namespace Atom.Rendering
{
    public abstract class Block
    {
        public abstract bool IsEditable { get; }

        public abstract bool IsEnabled { get; }

        public abstract string DisplayValue { get; set; }
    }
}
