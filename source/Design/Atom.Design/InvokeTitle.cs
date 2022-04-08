using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Atom.Design
{
    public class InvokeTitle : ItemsControl, IEnumerable<UIElement>
    {
        static InvokeTitle()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(InvokeTitle), new FrameworkPropertyMetadata(typeof(InvokeTitle)));
        }

        public InvokeTitle(string methodTitle, ArgumentCollection arguments)
        {
            Build(methodTitle, arguments);
        }
        
        public IEnumerator<UIElement> GetEnumerator()
        {
            return Items.OfType<UIElement>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
        }

        private void Build(string methodTitle, ArgumentCollection arguments)
        {
            TitleReader reader = new TitleReader(methodTitle);
            while (reader.MoveNext())
            {
                if (reader.IsParameter)
                {
                    string parameterName = reader.Content;
                    Argument argument = arguments[parameterName];
                    Items.Add(argument);
                }
                else
                {
                    TitleText titleText = new TitleText(reader.Content);
                    Items.Add(titleText);
                }
            }
        }
    }
}
