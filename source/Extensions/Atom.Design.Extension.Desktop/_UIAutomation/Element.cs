using System;
using System.Collections.Generic;
using System.Windows.Automation;

namespace Atom.Design.Extension.Desktop
{
    public class Element : LazyReadOnlyCollection<Element>, IEquatable<Element>
    {
        private readonly int[] _runtimeId;
        private readonly Lazy<Element> _parent;

        public Element(AutomationElement automationElement, Element parent)
            : this(automationElement, parent, true)
        {
        }

        public Element(AutomationElement automationElement)
            : this(automationElement, null, false)
        {
        }

        private Element(AutomationElement automationElement, Element parent, bool parentSpecified)
        {
            _runtimeId = automationElement.GetRuntimeId();
            AutomationElement = automationElement;
            Properties = new ElementPropertyCollection(AutomationElement);
            Patterns = new ElementPatternCollection(AutomationElement);
            if (parentSpecified)
            {
                _parent = new Lazy<Element>(() => parent);
            }
            else
            {
                _parent = new Lazy<Element>(() => ElementLocator.GetParent(this));
            }
        }

        internal AutomationElement AutomationElement { get; private set; }

        public Element Parent
        {
            get { return _parent.Value; }
        }
        
        public ElementPropertyCollection Properties { get; private set; }

        public ElementPatternCollection Patterns { get; private set; }

        protected override IList<Element> Initialize()
        {
            return ElementLocator.GetChildren(this);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Element);
        }

        public bool Equals(Element other)
        {
            if (other == null)
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            int[] otherRuntimeId = other._runtimeId;
            return Automation.Compare(_runtimeId, otherRuntimeId);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                for (int i = 0; i < _runtimeId.Length; i++)
                {
                    hash = hash * 23 + _runtimeId[i];
                }
                return hash;
            }
        }
    }
}
