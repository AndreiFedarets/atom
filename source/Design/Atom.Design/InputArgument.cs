using Atom.Design.Reflection;
using Atom.Design.Reflection.Metadata;
using System.Windows;

namespace Atom.Design
{
    public class InputArgument : Argument, IValueConsumer
    {
        public static readonly DependencyProperty ValueProperty;
        private static readonly DependencyPropertyKey ValueNamePropertyKey;
        public static readonly DependencyProperty ValueNameProperty;

        static InputArgument()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(InputArgument), new FrameworkPropertyMetadata(typeof(InputArgument)));
            ValueProperty = DependencyProperty.Register("Value", typeof(BaseValue), typeof(InputArgument), new PropertyMetadata(null, OnValuePropertyChanged));
            ValueNamePropertyKey = DependencyProperty.RegisterReadOnly("ValueName", typeof(string), typeof(InputArgument), new PropertyMetadata(null));
            ValueNameProperty = ValueNamePropertyKey.DependencyProperty;
        }

        public InputArgument(ParameterReference parameter)
            : base(parameter)
        {
            UpdateValueName();
        }

        //TODO: [on set] validate Source.ValueType is compatible with Parameter.ParameterType
        public BaseValue Value
        {
            get { return (BaseValue)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public string ValueName
        {
            get { return (string)GetValue(ValueNameProperty); }
            set { SetValue(ValueNamePropertyKey, value); }
        }

        private void UpdateValueName()
        {
            string valueName = string.Empty;
            if (Value == null)
            {
                valueName = "<___>";
            }
            else if (Value is VariableValue)
            {
                valueName = (Value as VariableValue).Name;
            }
            else if (Value is PropertyValue)
            {
                valueName = (Value as PropertyValue).Reference.Name;
            }
            else if (Value is ConstantValue)
            {
                valueName = $"<{Parameter.Name}>";
            }
            ValueName = valueName;
        }

        private static void OnValuePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs eventArgs)
        {
            InputArgument argument = (InputArgument)sender;
            argument.UpdateValueName();
            DesignerEvents.RaiseDesignerChanged(argument);
        }
    }
}
