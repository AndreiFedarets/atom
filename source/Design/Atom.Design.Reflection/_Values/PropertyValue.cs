using Atom.Design.Reflection.Metadata;

namespace Atom.Design.Reflection
{
    public class PropertyValue : BaseValue
    {
        public PropertyValue(PropertyReference reference)
            : base(reference.PropertyType)
        {
            Reference = reference;
        }

        public PropertyReference Reference { get; private set; }
    }
}
