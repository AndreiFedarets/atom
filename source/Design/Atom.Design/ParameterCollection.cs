using Atom.Design.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Atom.Design
{
    public sealed class ParameterCollection : ItemsControl, IEnumerable<Parameter>, IValueScope
    {
        public IEnumerable<IValueSource> Sources
        {
            get { return Items.OfType<IValueSource>(); }
        }

        public IEnumerable<IValueConsumer> Consumers
        {
            get { return Items.OfType<IValueConsumer>(); }
        }

        public void Add(Parameter parameter)
        {
            if (!Items.Contains(parameter))
            {
                Items.Add(parameter);
                DesignerEvents.RaiseDesignerChanged(this);
            }
        }

        public void Remove(Parameter parameter)
        {
            if (Items.Contains(parameter))
            {
                Items.Remove(parameter);
                DesignerEvents.RaiseDesignerChanged(this);
            }
        }

        public IEnumerator<Parameter> GetEnumerator()
        {
            return Items.OfType<Parameter>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
