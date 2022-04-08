using System.Windows;

namespace Atom.Design
{
    public sealed class Workflow : Method
    {
        static Workflow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Workflow), new FrameworkPropertyMetadata(typeof(Workflow)));
        }

        public Workflow()
        {
            Title = new SimpleTitle();
        }

        public SimpleTitle Title { get; private set; }
    }
}
