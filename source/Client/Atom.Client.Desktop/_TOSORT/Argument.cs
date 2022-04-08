namespace Atom.Runtime
{
    internal sealed class Argument : IArgument
    {
        private IValueBinding _binding;

        public Argument(ArgumentMetadata metadata)
        {
            Metadata = metadata;
        }

        public ArgumentMetadata Metadata { get; private set; }

        public IValueBinding ValueBinding
        {
            get
            {
                if (_binding == null)
                {
                    //Default binding for Input parameter is EmptyBinding, for Output the only available binding is ExactValueBinding
                    _binding = Metadata.ParameterMetadata.Direction == Direction.Input ? EmptyValueBinding.Instance : new ExactValueBinding();
                }
                return _binding;
            }
            set
            {
                //Setting of binding in case of Output parameter is not possible
                if (Metadata.ParameterMetadata.Direction == Direction.Input)
                {
                    _binding = value;
                }
            }
        }

        //public Type Type
        //{
        //    get { return _parameter.Type; }
        //}

        //public IParameter GetParameter()
        //{
        //    return _parameter;
        //}
    }
}
