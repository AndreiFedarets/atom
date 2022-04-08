namespace Atom.Rendering
{
    public class OutputArgumentBlock : Block
    {
        private readonly IArgument _argument;

        public OutputArgumentBlock(IArgument argument)
        {
            _argument = argument;
        }

        public string ArgumentName
        {
            get { return _argument.Name; }
            set { _argument.Rename(value); }
        }

        public Direction Direction
        {
            get { return _argument.Direction; }
        }

        public override bool IsEditable
        {
            get { return true; }
        }

        public override bool IsEnabled
        {
            get { return true; }
        }

        public override string DisplayValue
        {
            get { return _argument.Name; }
            set { _argument.Rename(value); }
        }
    }
}
