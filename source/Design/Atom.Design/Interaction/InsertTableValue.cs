using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Linq;
using Atom.Design.Services;

namespace Atom.Design.Interaction
{
    [TemplatePart(Name = ValueTypeComboBoxPartName, Type = typeof(ComboBox))]
    public class InsertTableValue : Thumb
    {
        private const string ValueTypeComboBoxPartName = "PART_ValueTypeComboBox";

        private readonly SelectTypeItem _selectTypeItem;
        private ComboBox _valueTypeComboBox;

        static InsertTableValue()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(InsertTableValue), new FrameworkPropertyMetadata(typeof(InsertTableValue)));
        }

        public InsertTableValue()
        {
            _selectTypeItem = new SelectTypeItem();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (_valueTypeComboBox != null)
            {
                _valueTypeComboBox.SelectionChanged -= OnValueTypeComboBoxSelectionChanged;
            }
            _valueTypeComboBox = GetTemplateChild(ValueTypeComboBoxPartName) as ComboBox;
            if (_valueTypeComboBox != null)
            {
                _valueTypeComboBox.SelectionChanged += OnValueTypeComboBoxSelectionChanged;
                _valueTypeComboBox.ItemsSource = new object[] { _selectTypeItem }.Concat(Services.Services.TypeService.GetExtensions());
            }
        }

        private void OnValueTypeComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ITypeExtension typeExtension = _valueTypeComboBox.SelectedItem as ITypeExtension;
            Table table = DesignerHelpers.GetParent<Table>(this);
            if (typeExtension != null && table != null)
            {
                typeExtension.InsertInteractive(table);
                _valueTypeComboBox.SelectedItem = _selectTypeItem;
            }
        }

        private class SelectTypeItem
        {
            public string DisplayName
            {
                get { return ""; }
            }
        }
    }
}
