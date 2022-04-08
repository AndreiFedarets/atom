using Atom.Design.Reflection;
using Atom.Design.Reflection.Metadata;
using System.Collections.Generic;
using System.Windows;
using System.Linq;

namespace Atom.Design
{
    public sealed class OutputArgument : Argument, IValueSource
    {
        private static readonly DependencyPropertyKey ValueNamePropertyKey;
        public static readonly DependencyProperty ValueNameProperty;

        static OutputArgument()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OutputArgument), new FrameworkPropertyMetadata(typeof(OutputArgument)));
            ValueNamePropertyKey = DependencyProperty.RegisterReadOnly("ValueName", typeof(string), typeof(OutputArgument), new PropertyMetadata(null));
            ValueNameProperty = ValueNamePropertyKey.DependencyProperty;
        }

        public OutputArgument(ParameterReference parameter)
            : base(parameter)
        {
            ValueName = parameter.Name;
        }

        public string ValueName
        { 
            get { return (string)GetValue(ValueNameProperty); }
            private set { SetValue(ValueNamePropertyKey, value); }
        }

        public BaseValue CreateValue()
        {
            return new VariableValue(Parameter.ParameterType, ValueName);
        }

        public bool Rename(string desiredName)
        {
            //BaseValue oldValue = CreateValue();
            //Method method = DesignerHelpers.GetParent<Method>(this);
            //if (!DesignerHelpers.CanRenameTo(this, method, desiredName))
            //{
            //    return false;
            //}
            if (string.IsNullOrEmpty(desiredName))
            {
                return false;
            }
            ValueName = desiredName;
            //BaseValue newValue = CreateValue();
            //DesignerHelpers.RebindConsumers(oldValue, newValue, method);
            DesignerEvents.RaiseDesignerChanged(this);
            return true;
            //throw new System.NotImplementedException();
        }
    }
}
