using Atom.Design.Reflection;
using Atom.Design.Reflection.Metadata;
using System.Windows;

namespace Atom.Design
{
    public sealed class InputParameter : Parameter, IValueSource
    {
        static InputParameter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(InputParameter), new FrameworkPropertyMetadata(typeof(InputParameter)));
        }

        public InputParameter(string valueName, TypeReference valueType)
            : base(valueName, valueType)
        {
        }

        public BaseValue CreateValue()
        {
            return new VariableValue(ValueType, ValueName);
        }
    }
}
