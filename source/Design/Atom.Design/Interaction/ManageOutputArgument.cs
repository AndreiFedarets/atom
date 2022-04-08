using System.Windows;
using System.Windows.Controls;

namespace Atom.Design.Interaction
{
    [TemplatePart(Name = RenameArgumentButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = DesiredNameTextBoxPartName, Type = typeof(TextBox))]
    public class ManageOutputArgument : ManageArgument
    {
        private const string RenameArgumentButtonPartName = "PART_RenameArgumentButton";
        private const string DesiredNameTextBoxPartName = "PART_DesiredNameTextBox";

        private Button _renameArgumentButton;
        private TextBox _desiredNameTextBox;

        static ManageOutputArgument()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ManageOutputArgument), new FrameworkPropertyMetadata(typeof(ManageOutputArgument)));
        }

        public new OutputArgument Argument
        {
            get { return (OutputArgument)base.Argument; }
            set { base.Argument = value; }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //PART_ManageArgumentButton
            if (_renameArgumentButton != null)
            {
                _renameArgumentButton.Click -= OnRenameArgumentButtonClick;
            }
            _renameArgumentButton = GetTemplateChild(RenameArgumentButtonPartName) as Button;
            if (_renameArgumentButton != null)
            {
                _renameArgumentButton.Click += OnRenameArgumentButtonClick;
            }
            //PART_DesiredNameTextBox
            _desiredNameTextBox = GetTemplateChild(DesiredNameTextBoxPartName) as TextBox;
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            _desiredNameTextBox.Text = Argument.ValueName;
        }

        private void OnRenameArgumentButtonClick(object sender, RoutedEventArgs e)
        {
            if (Argument.Rename(_desiredNameTextBox.Text))
            {
                Close();
            }
        }
    }
}
