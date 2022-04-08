using Atom.Design.Reflection.Metadata;
using System.Windows;
using System.Windows.Controls;

namespace Atom.Design
{
    [TemplatePart(Name = RemoveParameterButtonPartName, Type = typeof(Button))]
    public abstract class Parameter : Control
    {
        private const string RemoveParameterButtonPartName = "PART_RemoveParameterButton";

        private static readonly DependencyPropertyKey ValueNamePropertyKey;
        public static readonly DependencyProperty ValueNameProperty;

        private Button _removeParameterButton;

        static Parameter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Parameter), new FrameworkPropertyMetadata(typeof(Parameter)));
            ValueNamePropertyKey = DependencyProperty.RegisterReadOnly("ValueName", typeof(string), typeof(InputParameter), new PropertyMetadata(string.Empty));
            ValueNameProperty = ValueNamePropertyKey.DependencyProperty;
        }

        public Parameter(string valueName, TypeReference valueType)
        {
            ValueType = valueType;
            ValueName = valueName;
        }

        public string ValueName
        {
            get { return (string)GetValue(ValueNameProperty); }
            private set { SetValue(ValueNamePropertyKey, value); }
        }

        public TypeReference ValueType { get; private set; }

        public void Rename(string desiredName)
        {
            DesignerEvents.RaiseDesignerChanged(this);
            ValueName = desiredName;
            throw new System.NotImplementedException();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (_removeParameterButton != null)
            {
                _removeParameterButton.Click -= OnRemoveParameterButtonClick;
            }
            _removeParameterButton = GetTemplateChild(RemoveParameterButtonPartName) as Button;
            if (_removeParameterButton != null)
            {
                _removeParameterButton.Click += OnRemoveParameterButtonClick;
            }
        }

        private void OnRemoveParameterButtonClick(object sender, RoutedEventArgs e)
        {
            ParameterCollection parent = DesignerHelpers.GetParent<ParameterCollection>(this);
            parent?.Remove(this);
        }
    }
}
