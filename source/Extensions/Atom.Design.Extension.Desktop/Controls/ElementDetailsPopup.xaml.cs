using Atom.Client.SystemHooks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace Atom.Design.Extension.Desktop.Controls
{
    public partial class ElementDetailsPopup
    {
        public static readonly DependencyProperty ElementPropertiesProperty;
        private static readonly DependencyPropertyKey ElementPropertiesPropertyKey;

        private readonly ObservableCollection<Tuple<string, object>> _elementProperties;

        static ElementDetailsPopup()
        {
            ElementPropertiesPropertyKey = DependencyProperty.RegisterReadOnly("ElementProperties", typeof(IEnumerable<Tuple<string, object>>), typeof(ElementDetailsPopup), new PropertyMetadata());
            ElementPropertiesProperty = ElementPropertiesPropertyKey.DependencyProperty;
        }

        private MouseHook _mouseHook;
        private Element _element;

        public ElementDetailsPopup()
        {
            InitializeComponent();
            _elementProperties = new ObservableCollection<Tuple<string, object>>();
            ElementProperties = _elementProperties;
            _mouseHook = new MouseHook(MouseCallback);
            IsVisibleChanged += OnIsVisibleChanged;
        }
        
        public event EventHandler<ElementEventArgs> InsertElement;

        public event EventHandler<ElementEventArgs> SyncronizeElement;

        public IEnumerable<Tuple<string, object>> ElementProperties
        {
            get { return (IEnumerable<Tuple<string, object>>)GetValue(ElementPropertiesProperty); }
            private set { SetValue(ElementPropertiesPropertyKey, value); }
        }

        public void Show(Element element)
        {
            _element = element;
            Show();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            IsVisibleChanged -= OnIsVisibleChanged;
            _mouseHook.Dispose();
        }

        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
            {
                _mouseHook.Subscribe();
            }
            else
            {
                _mouseHook.Unsubscribe();
            }
        }

        private bool MouseCallback(MouseEventType eventType, MouseEventData eventData)
        {
            if (eventType == MouseEventType.LeftButtonDown)
            {
                if (AddButton.IsMouseOver)
                {
                    InsertElement?.Invoke(this, new ElementEventArgs(_element));
                    return true;
                }
                if (SynchronizeButton.IsMouseOver)
                {
                    SyncronizeElement?.Invoke(this, new ElementEventArgs(_element));
                    return true;
                }
            }
            return false;
        }

        private void BuildElementProperties()
        {
            _elementProperties.Clear();
            if (_element == null)
            {
                return;
            }
            _elementProperties.Add(new Tuple<string, object>("Name: ", _element.Properties.Name));
            _elementProperties.Add(new Tuple<string, object>("AutomationId: ", _element.Properties.AutomationId));
            _elementProperties.Add(new Tuple<string, object>("Class Name: ", _element.Properties.ClassName));
            _elementProperties.Add(new Tuple<string, object>("Control Type: ", _element.Properties.ControlType.LocalizedControlType));
        }

        private static void OnElementPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs eventArgs)
        {
            ElementDetailsPopup detailsPopup = sender as ElementDetailsPopup;
            if (detailsPopup != null)
            {
                detailsPopup.BuildElementProperties();
            }
        }
    }
}
