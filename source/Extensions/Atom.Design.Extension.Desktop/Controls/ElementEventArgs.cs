using System;

namespace Atom.Design.Extension.Desktop.Controls
{
    public class ElementEventArgs : EventArgs
    {
        public ElementEventArgs(Element element)
        {
            Element = element;
        }

        public Element Element { get; private set; }
    }
}
