using Atom.Design.Reflection;
using Atom.Design.Reflection.Metadata;
using System.Windows;

namespace Atom.Design
{
    public sealed class OutputParameter : Parameter, IValueConsumer
    {
        public static readonly DependencyProperty ValueProperty;

        static OutputParameter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OutputParameter), new FrameworkPropertyMetadata(typeof(OutputParameter)));
            ValueProperty = DependencyProperty.Register("Value", typeof(Reflection.BaseValue), typeof(OutputParameter), new PropertyMetadata(null, OnValuePropertyChanged));
        }

        public OutputParameter(string valueName, TypeReference valueType)
            : base(valueName, valueType)
        {
        }

        //TODO: [on set] validate Source.ValueType is compatible with ValueType
        public BaseValue Value
        {
            get { return (BaseValue)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static void OnValuePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs eventArgs)
        {
            OutputParameter parameter = (OutputParameter)sender;
            DesignerEvents.RaiseDesignerChanged(parameter);
        }
    }
}
