using Atom.Design.Hosting;
using Atom.Design.Reflection;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Atom.Design
{
    public abstract class Method : Control, IObjectDesigner, IValueScopeCollection
    {
        static Method()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Method), new FrameworkPropertyMetadata(typeof(Method)));
        }

        public Method()
        {
            Instructions = new InstructionCollection();
        }

        public IDocument Document { get; set; }

        public InstructionCollection Instructions { get; private set; }

        public virtual IEnumerable<IValueScope> Scopes
        {
            get { return Instructions.Scopes; }
        }

        public virtual IEnumerable<IValueSource> Sources
        {
            get { return Instructions.Sources; }
        }

        public virtual IEnumerable<IValueConsumer> Consumers
        {
            get { return Instructions.Consumers; }
        }
    }
}
