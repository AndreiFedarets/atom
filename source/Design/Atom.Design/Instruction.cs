using Atom.Design.Reflection;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Atom.Design
{
    [TemplatePart(Name = RemoveInstructionButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = MoveInstructionUpButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = MoveInstructionDownButtonPartName, Type = typeof(Button))]
    public abstract class Instruction : Thumb, IValueScope
    {
        private const string RemoveInstructionButtonPartName = "PART_RemoveInstructionButton";
        private const string MoveInstructionUpButtonPartName = "PART_MoveInstructionUpButton";
        private const string MoveInstructionDownButtonPartName = "PART_MoveInstructionDownButton";

        public static readonly DependencyProperty IsSelectedProperty;
        private static readonly DependencyPropertyKey IsSelectedPropertyKey;

        private Button _removeButton;
        private Button _moveUpButton;
        private Button _moveDownButton;

        static Instruction()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Instruction), new FrameworkPropertyMetadata(typeof(Instruction)));
            IsSelectedPropertyKey = DependencyProperty.RegisterReadOnly("IsSelected", typeof(bool), typeof(Instruction), new PropertyMetadata(false));
            IsSelectedProperty = IsSelectedPropertyKey.DependencyProperty;
        }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            internal set { SetValue(IsSelectedPropertyKey, value); }
        }

        public abstract IEnumerable<IValueSource> Sources { get; }

        public abstract IEnumerable<IValueConsumer> Consumers { get; }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //Remove
            if (_removeButton != null)
            {
                _removeButton.Click -= OnRemoveInstructionButtonClick;
            }
            _removeButton = GetTemplateChild(RemoveInstructionButtonPartName) as Button;
            if (_removeButton != null)
            {
                _removeButton.Click += OnRemoveInstructionButtonClick;
            }
            //Move Up
            if (_moveUpButton != null)
            {
                _moveUpButton.Click -= OnMoveInstructionUpButtonClick;
            }
            _moveUpButton = GetTemplateChild(MoveInstructionUpButtonPartName) as Button;
            if (_moveUpButton != null)
            {
                _moveUpButton.Click += OnMoveInstructionUpButtonClick;
            }
            //Move Down
            if (_moveDownButton != null)
            {
                _moveDownButton.Click -= OnMoveInstructionDownButtonClick;
            }
            _moveDownButton = GetTemplateChild(MoveInstructionDownButtonPartName) as Button;
            if (_moveDownButton != null)
            {
                _moveDownButton.Click += OnMoveInstructionDownButtonClick;
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            InstructionCollection parent = DesignerHelpers.GetParent<InstructionCollection>(this);
            parent?.Select(this);
            base.OnMouseDown(e);
        }

        private void OnRemoveInstructionButtonClick(object sender, RoutedEventArgs e)
        {
            InstructionCollection parent = DesignerHelpers.GetParent<InstructionCollection>(this);
            parent?.Remove(this);
        }

        private void OnMoveInstructionUpButtonClick(object sender, RoutedEventArgs e)
        {
            InstructionCollection parent = DesignerHelpers.GetParent<InstructionCollection>(this);
            parent?.MoveUp(this);
        }

        private void OnMoveInstructionDownButtonClick(object sender, RoutedEventArgs e)
        {
            InstructionCollection parent = DesignerHelpers.GetParent<InstructionCollection>(this);
            parent?.MoveDown(this);
        }
    }
}
