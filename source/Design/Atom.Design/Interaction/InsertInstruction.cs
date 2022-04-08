using Atom.Design.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace Atom.Design.Interaction
{
    [TemplatePart(Name = InsertButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = MethodsListViewPartName, Type = typeof(ListView))]
    public sealed class InsertInstruction : Thumb
    {
        private const string InsertButtonPartName = "PART_InsertButton";
        private const string MethodsListViewPartName = "PART_MethodsListView";

        private static readonly DependencyPropertyKey IsSearchingPropertyKey;
        public static readonly DependencyProperty IsSearchingProperty;
        public static readonly DependencyProperty SearchTextProperty;

        private Button _insertButton;
        private ListView _methodsListView;

        static InsertInstruction()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(InsertInstruction), new FrameworkPropertyMetadata(typeof(InsertInstruction)));
            IsSearchingPropertyKey = DependencyProperty.RegisterReadOnly("IsSearching", typeof(bool), typeof(InsertInstruction), new PropertyMetadata(false));
            IsSearchingProperty = IsSearchingPropertyKey.DependencyProperty;
            SearchTextProperty = DependencyProperty.Register("SearchText", typeof(string), typeof(InsertInstruction), new PropertyMetadata(string.Empty, OnSearchTextPropertyChanged));
        }

        public InsertInstruction()
        {
            InstructionTypes = new[] { InstructionType.Invoke, InstructionType.Assert };
            SelectedInstructionType = InstructionTypes.First();
            PreviewKeyDown += OnPreviewKeyDown;
        }

        public bool IsSearching
        {
            get { return (bool)GetValue(IsSearchingProperty); }
            private set { SetValue(IsSearchingPropertyKey, value); }
        }

        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        public IEnumerable<InstructionType> InstructionTypes { get; private set; }

        public InstructionType SelectedInstructionType { get; set; }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (_insertButton != null)
            {
                _insertButton.Click -= OnInsertButtonClick;
            }
            _insertButton = GetTemplateChild(InsertButtonPartName) as Button;
            if (_insertButton != null)
            {
                _insertButton.Click += OnInsertButtonClick;
            }
            _methodsListView = GetTemplateChild(MethodsListViewPartName) as ListView;
        }

        private bool MethodFilter(object item)
        {
            string searchText = SearchText;
            if (searchText.Equals(" "))
            {
                return true;
            }
            IMethod method = (IMethod)item;
            return method.Title.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void OnInsertButtonClick(object sender, RoutedEventArgs e)
        {
            InsertSelectedMethod();
        }

        private void InsertSelectedMethod()
        {
            IMethod selectedMethod = _methodsListView?.SelectedItem as IMethod;
            if (selectedMethod != null)
            {
                InstructionCollection instructions = DesignerHelpers.GetParent<InstructionCollection>(this);
                if (instructions != null)
                {
                    InvokeInstruction instruction = new InvokeInstruction(selectedMethod);
                    instructions.Add(instruction);
                    instruction.AutoBindArguments();
                }
                SearchText = string.Empty;
            }
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (IsSearching)
            {
                switch (e.Key)
                {
                    case Key.Up:
                        if (_methodsListView != null && _methodsListView.SelectedIndex > 0)
                        {
                            _methodsListView.SelectedIndex--;
                        }
                        break;
                    case Key.Down:
                        if (_methodsListView != null && _methodsListView.SelectedIndex < _methodsListView.Items.Count)
                        {
                            _methodsListView.SelectedIndex++;
                        }
                        break;
                    case Key.Enter:
                        InsertSelectedMethod();
                        break;
                    case Key.Escape:
                        SearchText = string.Empty;
                        break;
                }
            }

        }

        private void RefreshSearch()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                IsSearching = false;
            }
            else if (IsSearching)
            {
                CollectionViewSource.GetDefaultView(_methodsListView.ItemsSource).Refresh();
            }
            else
            {
                IObjectDesigner parent = DesignerHelpers.GetParent<IObjectDesigner>(this);
                if (parent != null)
                {
                    Hosting.IDocument document = parent.Document;
                    _methodsListView.ItemsSource = Services.Services.ObjectExplorer.GetAvailableActions(document.Project);
                    CollectionViewSource.GetDefaultView(_methodsListView.ItemsSource).Filter = MethodFilter;
                    IsSearching = true;
                }
            }
        }
        
        private static void OnSearchTextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs eventArgs)
        {
            InsertInstruction insertInstruction = (InsertInstruction)sender;
            insertInstruction.RefreshSearch();
        }
    }
}
