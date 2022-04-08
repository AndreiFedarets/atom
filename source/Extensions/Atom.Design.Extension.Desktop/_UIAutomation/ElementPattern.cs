using System.Windows.Automation;

namespace Atom.Design.Extension.Desktop
{
    public class ElementPattern
    {
        private readonly AutomationElement _automationElement;
        private readonly AutomationPattern _automationPattern;
        private readonly BasePattern _basePattern;

        public ElementPattern(AutomationElement automationElement, AutomationPattern automationPattern)
        {
            _automationElement = automationElement;
            _automationPattern = automationPattern;
            _basePattern = (BasePattern)_automationElement.GetCurrentPattern(_automationPattern);
            DisplayName = Automation.PatternName(_automationPattern);
        }

        public string DisplayName { get; private set; }

        public int Id
        {
            get { return _automationPattern.Id; }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            ElementPattern other = obj as ElementPattern;
            if (other == null)
            {
                return false;
            }
            bool equals = _basePattern.Equals(other._basePattern);
            return equals;
        }

        public bool Equals(ElementPattern other)
        {
            return Equals((object)other);
        }

        public override int GetHashCode()
        {
            return _basePattern.GetHashCode();
        }
    }
}
