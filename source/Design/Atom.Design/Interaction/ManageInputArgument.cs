using Atom.Design.Reflection;
using Atom.Design.Reflection.Metadata;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Atom.Design.Interaction
{
    [TemplatePart(Name = ApplyValueButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = ClearValueButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = ValueSourceListBoxPartName, Type = typeof(ListView))]
    [TemplatePart(Name = ValueScopeComboBoxPartName, Type = typeof(ComboBox))]
    public class ManageInputArgument : ManageArgument
    {
        private const string ApplyValueButtonPartName = "PART_ApplyValueButton";
        private const string ClearValueButtonPartName = "PART_ClearValueButton";
        private const string ValueSourceListBoxPartName = "PART_ValueSourceListBox";
        private const string ValueScopeComboBoxPartName = "PART_ValueScopeComboBox";

        //public static DependencyProperty ValueSourceKindProperty;

        private Button _applyValueButton;
        private Button _clearValueButton;
        private ListView _valueSourceListView;
        private ComboBox _valueScopeComboBox;

        static ManageInputArgument()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ManageInputArgument), new FrameworkPropertyMetadata(typeof(ManageInputArgument)));
        }

        public new InputArgument Argument
        {
            get { return (InputArgument)base.Argument; }
            set { base.Argument = value; }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //PART_SelectValueButton
            if (_applyValueButton != null)
            {
                _applyValueButton.Click -= OnApplyValueButtonClick;
            }
            _applyValueButton = GetTemplateChild(ApplyValueButtonPartName) as Button;
            if (_applyValueButton != null)
            {
                _applyValueButton.Click += OnApplyValueButtonClick;
            }
            //PART_ClearValueButton
            if (_clearValueButton != null)
            {
                _clearValueButton.Click -= OnClearValueButtonClick;
            }
            _clearValueButton = GetTemplateChild(ClearValueButtonPartName) as Button;
            if (_clearValueButton != null)
            {
                _clearValueButton.Click += OnClearValueButtonClick;
            }
            //PART_ValueSourceListBox
            if (_valueSourceListView != null)
            {
                _valueSourceListView.MouseDoubleClick -= OnValueSourceListViewMouseDoubleClick;
            }
            _valueSourceListView = GetTemplateChild(ValueSourceListBoxPartName) as ListView;
            if (_valueSourceListView != null)
            {
                _valueSourceListView.MouseDoubleClick += OnValueSourceListViewMouseDoubleClick;
            }
            //PART_ValueSourceScopeComboBox
            if (_valueScopeComboBox != null)
            {
                //_valueSourceScopeComboBox.DropDownOpened
                _valueScopeComboBox.SelectionChanged -= OnValueScopeComboBoxSelectionChanged;
            }
            _valueScopeComboBox = GetTemplateChild(ValueScopeComboBoxPartName) as ComboBox;
            if (_valueScopeComboBox != null)
            {
                _valueScopeComboBox.SelectionChanged += OnValueScopeComboBoxSelectionChanged;
            }
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            LoadValueScopes();
        }

        private void LoadValueScopes()
        {
            List<object> valueScopes = new List<object>();
            Method method = DesignerHelpers.GetParent<Method>(this);
            if (method != null)
            {
                valueScopes.Add(new LocalValueScope(method, Argument));
                valueScopes.AddRange(Services.Services.ObjectExplorer.GetAvailableTables(method.Document.Project));
            }
            _valueScopeComboBox.ItemsSource = valueScopes;
            _valueScopeComboBox.SelectedItem = valueScopes.FirstOrDefault();
        }

        private void ApplySelectedValue()
        {
            IValueSource valueSource = _valueSourceListView.SelectedItem as IValueSource;
            if (valueSource != null)
            {
                Argument.Value = valueSource.CreateValue();
                Close();
            }
        }
        
        private void OnValueScopeComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IValueScope sourceValueScope = _valueScopeComboBox.SelectedItem as IValueScope;
            if (sourceValueScope != null)
            {
                if (sourceValueScope is ITable)
                {

                }
                _valueSourceListView.ItemsSource = sourceValueScope.GetSources(Argument.Parameter.ParameterType);
            }
        }

        private void OnClearValueButtonClick(object sender, RoutedEventArgs e)
        {
            Argument.Value = null;
            Close();
        }

        private void OnApplyValueButtonClick(object sender, RoutedEventArgs e)
        {
            ApplySelectedValue();
        }

        private void OnValueSourceListViewMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ApplySelectedValue();
        }

        private class LocalValueScope : IValueScope
        {
            private readonly Instruction _instruction;
            private readonly Method _method;
            private readonly InputArgument _argument;

            public LocalValueScope(Method method, InputArgument argument)
            {
                _method = method;
                _argument = argument;
                _instruction = DesignerHelpers.GetParent<Instruction>(argument);
            }

            public string Title
            {
                get { return $"Current {_method.GetType().Name}"; }
            }

            public IEnumerable<IValueConsumer> Consumers
            {
                get { return Enumerable.Empty<IValueConsumer>(); }
            }

            public IEnumerable<IValueSource> Sources
            {
                get { return DesignerHelpers.FindLocalValueSources(_argument.ValueType, _instruction, _method); }
            }
        }
    }
}
