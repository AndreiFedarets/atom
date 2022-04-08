namespace Atom.Rendering
{
    public class InputArgumentBlock : Block
    {
        private readonly IArgument _argument;

        public InputArgumentBlock(IArgument argument)
        {
            _argument = argument;
        }

        public override bool IsEditable
        {
            get { return true; }
        }

        public override bool IsEnabled
        {
            get { return false; }
        }

        public override string DisplayValue
        {
            get
            {
                //TODO: refactor this if-blocks
                if (_argument.ValueBinding is IDataSourceValueBinding)
                {
                    return ((IDataSourceValueBinding) _argument.ValueBinding).ValueName;
                }
                if (_argument.ValueBinding is IExactValueBinding)
                {
                    object value = ((IExactValueBinding) _argument.ValueBinding).Value;
                    return value == null ? string.Empty : value.ToString();
                }
                return string.Empty;
            }
            set
            {
                
            }
        }
    }
}
