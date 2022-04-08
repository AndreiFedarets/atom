using Atom.Design.Reflection;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System;

namespace Atom.Design
{
    public sealed class InvokeInstruction : Instruction
    {
        static InvokeInstruction()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(InvokeInstruction), new FrameworkPropertyMetadata(typeof(InvokeInstruction)));
        }

        public InvokeInstruction(IMethod method)
        {
            Method = method;
            Arguments = new ArgumentCollection(this);
            Title = new InvokeTitle(method.Title, Arguments);
        }

        public IMethod Method { get; private set; }

        public InvokeTitle Title { get; private set; }

        public ArgumentCollection Arguments { get; private set; }

        public override IEnumerable<IValueSource> Sources
        {
            get { return Arguments.OfType<IValueSource>(); }
        }

        public override IEnumerable<IValueConsumer> Consumers
        {
            get { return Arguments.OfType<IValueConsumer>(); }
        }

        public void AutoBindArguments()
        {
            Method method = DesignerHelpers.GetParent<Method>(this);
            if (method != null)
            {
                DesignerHelpers.BindLocalValueConsumers(this, method);
            }
        }
    }
}
