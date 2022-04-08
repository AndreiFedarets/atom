using System.Windows;
using System.Windows.Controls;

namespace Atom.Design
{
    public class SimpleTitle : Control
    {
        public static readonly DependencyProperty TextProperty;

        static SimpleTitle()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SimpleTitle), new FrameworkPropertyMetadata(typeof(SimpleTitle)));
            TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(SimpleTitle), new PropertyMetadata(string.Empty, OnTextPropertyChanged));
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private static void OnTextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs eventArgs)
        {
            SimpleTitle titleText = (SimpleTitle)sender;
            DesignerEvents.RaiseDesignerChanged(titleText);
        }
    }
}
