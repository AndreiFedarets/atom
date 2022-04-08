using Atom.Design.Interaction;
using Atom.Design.Reflection.Metadata;
using System.Windows;
using System.Windows.Controls;

namespace Atom.Design
{
    [TemplatePart(Name = ManageArgumentButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = ManageArgumentPopupPartName, Type = typeof(ManageArgument))]
    public abstract class Argument : Control
    {
        private const string ManageArgumentButtonPartName = "PART_ManageArgumentButton";
        private const string ManageArgumentPopupPartName = "PART_ManageArgumentPopup";

        private Button _manageArgumentButton;
        private ManageArgument _manageArgument;

        static Argument()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Argument), new FrameworkPropertyMetadata(typeof(Argument)));
        }

        protected Argument(ParameterReference parameter)
        {
            Parameter = parameter;
        }

        public ParameterReference Parameter { get; private set; }

        public TypeReference ValueType
        {
            get { return Parameter.ParameterType; }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //PART_ManageArgumentButton
            if (_manageArgumentButton != null)
            {
                _manageArgumentButton.Click -= OnManageArgumentButtonClick;
            }
            _manageArgumentButton = GetTemplateChild(ManageArgumentButtonPartName) as Button;
            if (_manageArgumentButton != null)
            {
                _manageArgumentButton.Click += OnManageArgumentButtonClick;
            }
            //PART_ManageArgumentPopup
            _manageArgument = GetTemplateChild(ManageArgumentPopupPartName) as ManageArgument;
        }

        private void OnManageArgumentButtonClick(object sender, RoutedEventArgs e)
        {
            if (_manageArgument != null)
            {
                _manageArgument.IsOpen = !_manageArgument.IsOpen;
            }
        }
    }
}
