using Atom.Design.Reflection.Metadata;
using System.Windows;
using System.Windows.Controls;

namespace Atom.Design
{
    //TODO: inherit from Variable
    [TemplatePart(Name = RemoveValueButtonPartName, Type = typeof(Button))]
    public sealed class TableValue : Control
    {
        public const string RemoveValueButtonPartName = "PART_RemoveValueButton";

        public static readonly DependencyProperty ValueProperty;

        private Button _removeValueButton;

        static TableValue()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TableValue), new FrameworkPropertyMetadata(typeof(TableValue)));
            ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(TableValue), new PropertyMetadata(null, OnValuePropertyChanged));
        }

        public TableValue(string name, TypeReference type)
            : this(name, type, null)
        {
        }

        public TableValue(string name, TypeReference type, object value)
        {
            ValueName = name;
            ValueType = type;
            Value = value;
        }

        public string ValueName { get; private set; }

        public TypeReference ValueType { get; private set; }

        //TODO: check value type is compatible with ValueType
        public object Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (_removeValueButton != null)
            {
                _removeValueButton.Click -= OnRemoveValueButtonClick;
            }
            _removeValueButton = GetTemplateChild(RemoveValueButtonPartName) as Button;
            if (_removeValueButton != null)
            {
                _removeValueButton.Click += OnRemoveValueButtonClick;
            }
        }

        private void OnRemoveValueButtonClick(object sender, RoutedEventArgs e)
        {
            Table parent = DesignerHelpers.GetParent<Table>(this);
            if (parent != null)
            {
                parent.Remove(this);
            }
        }

        private static void OnValuePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs eventArgs)
        {
            TableValue tableValue = (TableValue)sender;
            DesignerEvents.RaiseDesignerChanged(tableValue);
        }
    }
}
