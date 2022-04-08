using System.Windows;
using System.Windows.Controls;

namespace Atom.Design
{
    public class TitleText : Control
    {
        public static readonly DependencyProperty ReadOnlyProperty;
        public static readonly DependencyProperty TextProperty;

        static TitleText()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TitleText), new FrameworkPropertyMetadata(typeof(TitleText)));
            ReadOnlyProperty = DependencyProperty.Register("ReadOnly", typeof(bool), typeof(TitleText), new PropertyMetadata(true));
            TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(TitleText), new PropertyMetadata(string.Empty, OnTextPropertyChanged));
        }

        private TitleText(bool readOnly, string text)
        {
            ReadOnly = readOnly;
            Text = text;
        }

        public TitleText()
            : this(false, string.Empty)
        {
        }

        public TitleText(string text)
            : this(true, text)
        {
        }

        public bool ReadOnly
        {
            get { return (bool)GetValue(ReadOnlyProperty); }
            set { SetValue(ReadOnlyProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        
        private static void OnTextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs eventArgs)
        {
            TitleText titleText = (TitleText)sender;
            DesignerEvents.RaiseDesignerChanged(titleText);
        }
    }
}
