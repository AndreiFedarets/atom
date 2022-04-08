using Atom.Design.Reflection;
using System.Collections.Generic;
using System.Windows;
using System.Linq;

namespace Atom.Design
{
    public sealed class Action : Method
    {
        static Action()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Action), new FrameworkPropertyMetadata(typeof(Action)));
        }

        public Action()
        {
            Parameters = new ParameterCollection();
            Title = new MethodTitle(Parameters);
        }

        public ParameterCollection Parameters { get; private set; }

        public MethodTitle Title { get; private set; }

        public override IEnumerable<IValueScope> Scopes
        {
            get { return new IValueScope[] { Parameters }.Concat(base.Scopes); }
        }

        public override IEnumerable<IValueSource> Sources
        {
            get { return Parameters.Sources.Concat(base.Sources); }
        }

        public override IEnumerable<IValueConsumer> Consumers
        {
            get { return Parameters.Consumers.Concat(base.Consumers); }
        }
    }
}
