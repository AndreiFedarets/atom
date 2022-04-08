using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Atom.Design.Interaction
{
    [TemplatePart(Name = CloseManageArgumentButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = ManageArgumentPopupPartName, Type = typeof(ManageArgument))]
    public class ManageArgument : Control
    {
        private const string CloseManageArgumentButtonPartName = "PART_CloseManageArgumentButton";
        private const string ManageArgumentPopupPartName = "PART_ManageArgumentPopup";

        public static readonly DependencyProperty ArgumentProperty;
        public static readonly DependencyProperty IsOpenProperty;
        public static readonly DependencyProperty PlacementTargetProperty;

        private Button _closeManageArgumentButton;
        private Popup _manageArgumentPopup;

        static ManageArgument()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ManageArgument), new FrameworkPropertyMetadata(typeof(ManageArgument)));
            ArgumentProperty = DependencyProperty.Register("Argument", typeof(Argument), typeof(ManageArgument), new PropertyMetadata(null));
            IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(ManageArgument), new PropertyMetadata(false));
            PlacementTargetProperty = DependencyProperty.Register("PlacementTarget", typeof(UIElement), typeof(ManageArgument), new PropertyMetadata(null));
        }

        public Argument Argument
        {
            get { return (Argument)GetValue(ArgumentProperty); }
            set { SetValue(ArgumentProperty, value); }
        }

        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public UIElement PlacementTarget
        {
            get { return (UIElement)GetValue(PlacementTargetProperty); }
            set { SetValue(PlacementTargetProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //PART_CloseManagePopupButton
            if (_closeManageArgumentButton != null)
            {
                _closeManageArgumentButton.Click -= OnCloseManageArgumentButtonClick;
            }
            _closeManageArgumentButton = GetTemplateChild(CloseManageArgumentButtonPartName) as Button;
            if (_closeManageArgumentButton != null)
            {
                _closeManageArgumentButton.Click += OnCloseManageArgumentButtonClick;
            }
            //PART_ManageArgumentPopup
            if (_manageArgumentPopup != null)
            {
                _manageArgumentPopup.Opened -= OnManageArgumentPopupOpened;
                _manageArgumentPopup.Closed -= OnManageArgumentPopupClosed;
            }
            _manageArgumentPopup = GetTemplateChild(ManageArgumentPopupPartName) as Popup;
            if (_manageArgumentPopup != null)
            {
                _manageArgumentPopup.Opened += OnManageArgumentPopupOpened;
                _manageArgumentPopup.Closed += OnManageArgumentPopupClosed;
            }
        }

        protected virtual void OnOpen()
        {

        }

        protected virtual void OnClose()
        {

        }

        protected void Close()
        {
            IsOpen = false;
        }

        private void OnManageArgumentPopupOpened(object sender, System.EventArgs e)
        {
            OnOpen();
        }

        private void OnManageArgumentPopupClosed(object sender, System.EventArgs e)
        {
            OnClose();
        }

        private void OnCloseManageArgumentButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
